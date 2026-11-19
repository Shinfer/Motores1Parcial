using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyChaser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private PlayerDarkness playerDarkness;

    [Header("Settings")]
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float maxVerticalDistance = 1.5f;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float spawnDuration = 5f;
    [SerializeField] private float respawnDelay = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource enemyAudio;
    [SerializeField] private AudioClip chaseSound;
    [SerializeField] private AudioSource heartbeatAudio;

    [Header("Animation")]
    [SerializeField] private Animator enemyAnimator;

    // Se modificó de 0.8f a 2f para permitir que termine
    // la animación de ataque antes de resolver la consecuencia.
    [SerializeField] private float attackDuration = 2f;

    private Renderer[] renderers;
    private Collider enemyCollider;

    // Estados del enemigo.
    private bool isVisible;
    private bool isChasing;
    private bool isAttacking;

    // Guarda si el golpe acertó en el momento del evento de animación.
    private bool attackHit;

    // Variables del parpadeo, actualmente desactivado en Update.
    private bool blinkOn = true;
    private float blinkTimer;

    // Se reutiliza para aparición, persecución y espera de reaparición.
    private float timer;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        enemyCollider = GetComponent<Collider>();

        // Si no se asignó un Animator en el Inspector, intenta encontrarlo.
        if (enemyAnimator == null)
            enemyAnimator = GetComponentInChildren<Animator>();

        // Comienza oculto y espera antes de aparecer.
        HideEnemy();
    }

    private void Update()
    {
        // Durante el ataque no se desplaza ni actualiza el temporizador.
        if (isAttacking)
            return;

        timer -= Time.deltaTime;

        // Mientras está oculto, espera para volver a aparecer.
        if (!isVisible)
        {
            if (timer <= 0f)
                SpawnEnemy();

            return;
        }

        if (isChasing)
        {
            ChasePlayer();

            // ChasePlayer puede iniciar un ataque.
            if (isAttacking)
                return;

            // Parpadeo desactivado para mantener visible al enemigo.
            // Blink();

            if (timer <= 0f)
                HideEnemy();

            return;
        }

        // Cuando está visible y no persigue, comprueba la distancia.
        float distance = Vector3.Distance(
            transform.position, player.position
        );

        // Solo detecta al jugador si está aproximadamente en el mismo piso.
        if (distance <= detectionDistance && IsOnSameFloor())
        {
            isChasing = true;
            timer = chaseDuration;

            if (enemyAnimator != null)
            {
                // enemyAnimator.Play("Walk");

                // Activa la transición de idle a Walk.
                enemyAnimator.SetBool("IsChasing", true);
            }

            // Informa al sistema de oscuridad que comenzó la persecución.
            playerDarkness.SetChasing(true);

            enemyAudio.Stop();
            enemyAudio.clip = chaseSound;
            enemyAudio.loop = true;
            enemyAudio.Play();

            heartbeatAudio.Play();
        }
        else if (timer <= 0f)
        {
            // Desaparece si termina su tiempo visible sin detectar al jugador.
            HideEnemy();
        }
    }

    private void SpawnEnemy()
    {
        // Selecciona un punto de aparición al azar.
        int index = Random.Range(0, spawnPoints.Length);

        transform.position = spawnPoints[index].position;
        transform.rotation = spawnPoints[index].rotation;

        foreach (Renderer r in renderers)
            r.enabled = true;

        enemyCollider.enabled = true;

        isVisible = true;
        isChasing = false;
        isAttacking = false;

        timer = spawnDuration;
        blinkOn = true;
        blinkTimer = 0.15f;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsChasing", false);

            // enemyAnimator.Play("idle", 0, 0f);
        }
    }

    private void HideEnemy()
    {
        // Oculta el modelo y desactiva su collider.
        foreach (Renderer r in renderers)
            r.enabled = false;

        enemyCollider.enabled = false;

        isVisible = false;
        isChasing = false;

        // Tiempo de espera antes de la siguiente aparición.
        timer = respawnDelay;

        if (enemyAnimator != null)
            enemyAnimator.SetBool("IsChasing", false);

        playerDarkness.SetChasing(false);
        enemyAudio.Stop();
        heartbeatAudio.Stop();
    }

    private void ChasePlayer()
    {
        // Si el jugador cambia de piso, termina la persecución.
        if (!IsOnSameFloor())
        {
            HideEnemy();
            return;
        }

        // Persigue al jugador solo en X y Z, sin subir ni bajar.
        Vector3 target = player.position;
        target.y = transform.position.y;

        Vector3 direction = target - transform.position;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            chaseSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(
            transform.position, target
        );

        // Inicia el ataque cuando llega al alcance configurado.
        if (distance <= attackDistance)
            StartCoroutine(AttackPlayer());
    }

    // El evento CheckHit del clip attack llama a este método
    // en el momento elegido para el impacto.
    public void CheckHit()
    {
        if (!isAttacking || player == null)
            return;

        // Comprueba distancia y dirección en el plano horizontal.
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;
        float angle = Vector3.Angle(transform.forward, direction);

        // Acierta si el jugador está dentro del alcance y delante:
        // hasta 45 grados a cada lado del frente del enemigo.
        // Es una comprobación por distancia y ángulo, no por contacto de la mano.
        attackHit = IsOnSameFloor() && distance <= attackDistance && angle <= 45f;
    }

    private IEnumerator AttackPlayer()
    {
        // Detiene la persecución y reinicia el resultado del golpe.
        isAttacking = true;
        isChasing = false;
        attackHit = false;

        if (enemyAnimator != null)
            enemyAnimator.SetTrigger("Attack");

        // Mantiene visible al enemigo durante el ataque.
        foreach (Renderer r in renderers)
            r.enabled = true;

        enemyAudio.Stop();
        heartbeatAudio.Stop();

        // Durante esta espera, el evento de animación llama a CheckHit.
        yield return new WaitForSeconds(attackDuration);

        // Usa el resultado registrado en el instante del impacto.
        if (attackHit)
        {
            SceneManager.LoadScene("DeathScene");
            yield break;
        }

        // Si el jugador cambió de piso durante el ataque, desaparece.
        if (!IsOnSameFloor())
        {
            isAttacking = false;
            HideEnemy();
            yield break;
        }

        // Si el golpe falló, retoma la persecución.
        isAttacking = false;
        isChasing = true;
        timer = chaseDuration;

        if (enemyAnimator != null)
        {
            enemyAnimator.ResetTrigger("Attack");
            enemyAnimator.SetBool("IsChasing", true);

            // Regresa directamente al estado Walk.
            enemyAnimator.Play("Walk", 0, 0f);
        }

        enemyAudio.clip = chaseSound;
        enemyAudio.loop = true;
        enemyAudio.Play();
        heartbeatAudio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        // También puede iniciar el ataque al entrar en contacto con el jugador.
        if (isChasing && !isAttacking && IsOnSameFloor() && other.CompareTag("Player"))
            StartCoroutine(AttackPlayer());
    }

    // Controla que el jugador no esté en otro piso.
    private bool IsOnSameFloor()
    {
        return Mathf.Abs(player.position.y - transform.position.y) <= maxVerticalDistance;
    }

    // Conservado para reutilizarlo si se desea.
    // Actualmente no se llama porque Blink() está comentado en Update.
    private void Blink()
    {
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0f)
        {
            blinkOn = !blinkOn;

            foreach (Renderer r in renderers)
                r.enabled = blinkOn;

            blinkTimer = 0.15f;
        }
    }
}