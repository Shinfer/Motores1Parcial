
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
    [SerializeField] private float attackDuration = 0.8f;

    private Renderer[] renderers;
    private Collider enemyCollider;

    private bool isVisible;
    private bool isChasing;
    private bool isAttacking;
    private bool blinkOn = true;

    private float timer;
    private float blinkTimer;

    // DEBUG: controla la frecuencia de los mensajes.
    private float debugTimer;

    // DEBUG: hueso que vamos a observar para comprobar
    // si el Animator realmente mueve el esqueleto.
    private Transform debugBone;
    private Quaternion lastBoneRotation;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        enemyCollider = GetComponent<Collider>();

        if (enemyAnimator == null)
            enemyAnimator = GetComponentInChildren<Animator>();

        // DEBUG: revisar las referencias y el Animator al iniciar.
        Debug.Log("[ENEMY DEBUG] ===== INICIO =====");
        Debug.Log("[ENEMY DEBUG] Objeto enemigo: " + gameObject.name);
        Debug.Log("[ENEMY DEBUG] Player: " +
            (player != null ? player.name : "SIN ASIGNAR"));
        Debug.Log("[ENEMY DEBUG] SpawnPoints: " + spawnPoints.Length);

        if (enemyAnimator != null)
        {
            Debug.Log("[ENEMY DEBUG] Animator encontrado: " +
                GetObjectPath(enemyAnimator.transform));

            Debug.Log("[ENEMY DEBUG] Animator enabled: " +
                enemyAnimator.enabled);

            Debug.Log("[ENEMY DEBUG] GameObject activo: " +
                enemyAnimator.gameObject.activeInHierarchy);

            Debug.Log("[ENEMY DEBUG] Controller: " +
                (enemyAnimator.runtimeAnimatorController != null
                    ? enemyAnimator.runtimeAnimatorController.name
                    : "SIN CONTROLLER"));

            Debug.Log("[ENEMY DEBUG] Avatar: " +
                (enemyAnimator.avatar != null
                    ? enemyAnimator.avatar.name
                    : "SIN AVATAR"));

            Debug.Log("[ENEMY DEBUG] Avatar valido: " +
                (enemyAnimator.avatar != null &&
                 enemyAnimator.avatar.isValid));

            Debug.Log("[ENEMY DEBUG] Apply Root Motion: " +
                enemyAnimator.applyRootMotion);

            // DEBUG: listar todos los parametros reales del Animator.
            foreach (AnimatorControllerParameter parameter
                in enemyAnimator.parameters)
            {
                Debug.Log("[ENEMY DEBUG] Parametro: " +
                    parameter.name + " | Tipo: " + parameter.type);
            }

            // DEBUG: listar los clips disponibles en el Controller.
            if (enemyAnimator.runtimeAnimatorController != null)
            {
                foreach (AnimationClip clip in
                    enemyAnimator.runtimeAnimatorController.animationClips)
                {
                    Debug.Log("[ENEMY DEBUG] Clip: " +
                        clip.name + " | Duracion: " +
                        clip.length + " | Loop: " +
                        clip.isLooping);
                }
            }
        }
        else
        {
            Debug.LogError("[ENEMY DEBUG] NO SE ENCONTRO ANIMATOR");
        }

        // DEBUG: comprobar los Skinned Mesh Renderer y sus huesos.
        SkinnedMeshRenderer[] meshes =
            GetComponentsInChildren<SkinnedMeshRenderer>();

        Debug.Log("[ENEMY DEBUG] Skinned Mesh Renderers: " +
            meshes.Length);

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            Debug.Log("[ENEMY DEBUG] Mesh: " +
                GetObjectPath(mesh.transform));

            Debug.Log("[ENEMY DEBUG] Root Bone: " +
                (mesh.rootBone != null
                    ? GetObjectPath(mesh.rootBone)
                    : "SIN ROOT BONE"));

            Debug.Log("[ENEMY DEBUG] Cantidad de huesos: " +
                mesh.bones.Length);

            // DEBUG: guardar un hueso para observar su movimiento.
            if (debugBone == null && mesh.bones.Length > 0)
                debugBone = mesh.bones[0];
        }

        if (debugBone != null)
        {
            lastBoneRotation = debugBone.localRotation;
            Debug.Log("[ENEMY DEBUG] Hueso observado: " +
                GetObjectPath(debugBone));
        }

        HideEnemy();
    }

    private void Update()
    {
        if (isAttacking)
            return;

        timer -= Time.deltaTime;

        if (!isVisible)
        {
            if (timer <= 0f)
                SpawnEnemy();

            return;
        }

        if (isChasing)
        {
            ChasePlayer();

            if (isAttacking)
                return;

            Blink();

            // DEBUG: revisar el Animator durante la persecucion.
            debugTimer -= Time.deltaTime;

            if (debugTimer <= 0f)
            {
                DebugAnimatorState();
                debugTimer = 0.5f;
            }

            if (timer <= 0f)
            {
                Debug.Log("[ENEMY DEBUG] FIN DE PERSECUCION");
                HideEnemy();
            }

            return;
        }

        float distance = Vector3.Distance(
            transform.position, player.position
        );

        if (distance <= detectionDistance)
        {
            isChasing = true;
            timer = chaseDuration;
            debugTimer = 0.5f;

            Debug.Log("[ENEMY DEBUG] ===== PERSECUCION =====");
            Debug.Log("[ENEMY DEBUG] Distancia al jugador: " +
                distance);

            if (enemyAnimator != null)
            {
                enemyAnimator.Play("Walk");

                Debug.Log("[ENEMY DEBUG] SetBool IsChasing = TRUE");

                Debug.Log("[ENEMY DEBUG] Valor leido: " +
                    enemyAnimator.GetBool("IsChasing"));
            }

            playerDarkness.SetChasing(true);

            enemyAudio.Stop();
            enemyAudio.clip = chaseSound;
            enemyAudio.loop = true;
            enemyAudio.Play();

            heartbeatAudio.Play();
        }
        else if (timer <= 0f)
        {
            HideEnemy();
        }
    }

    private void SpawnEnemy()
    {
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

        Debug.Log("[ENEMY DEBUG] ===== SPAWN =====");
        Debug.Log("[ENEMY DEBUG] Spawn elegido: " +
            spawnPoints[index].name);

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("IsChasing", false);
            //enemyAnimator.Play("idle", 0, 0f);

            Debug.Log("[ENEMY DEBUG] Orden enviada: Play idle");
        }
    }

    private void HideEnemy()
    {
        foreach (Renderer r in renderers)
            r.enabled = false;

        enemyCollider.enabled = false;

        isVisible = false;
        isChasing = false;

        timer = respawnDelay;

        if (enemyAnimator != null)
            enemyAnimator.SetBool("IsChasing", false);

        playerDarkness.SetChasing(false);
        enemyAudio.Stop();
        heartbeatAudio.Stop();

        // DEBUG: confirmar la desaparicion.
        Debug.Log("[ENEMY DEBUG] Enemigo oculto");
    }

    private void ChasePlayer()
    {
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

        if (distance <= attackDistance)
            StartCoroutine(AttackPlayer());
    }

    private IEnumerator AttackPlayer()
    {
        Debug.Log("[ENEMY DEBUG] ===== ATAQUE =====");

        isAttacking = true;
        isChasing = false;

        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Attack");
            Debug.Log("[ENEMY DEBUG] Trigger Attack enviado");
        }

        foreach (Renderer r in renderers)
            r.enabled = true;

        enemyAudio.Stop();
        heartbeatAudio.Stop();

        Debug.Log("[ENEMY DEBUG] Esperando animacion de ataque");

        yield return new WaitForSecondsRealtime(attackDuration);

        Debug.Log("[ENEMY DEBUG] Cargando DeathScene");

        SceneManager.LoadScene("DeathScene");
    }

    private void OnTriggerEnter(Collider other)
    {
        // DEBUG: comprobar que el collider detecte al jugador.
        Debug.Log("[ENEMY DEBUG] Contacto con: " + other.name);

        if (isChasing && !isAttacking && other.CompareTag("Player"))
            StartCoroutine(AttackPlayer());
    }

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

    // DEBUG: informacion sobre el estado real del Animator.
    private void DebugAnimatorState()
    {
        if (enemyAnimator == null)
            return;

        AnimatorStateInfo state =
            enemyAnimator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("[ENEMY DEBUG] ----- ESTADO -----");

        Debug.Log("[ENEMY DEBUG] Animator: " +
            GetObjectPath(enemyAnimator.transform));

        Debug.Log("[ENEMY DEBUG] IsChasing: " +
            enemyAnimator.GetBool("IsChasing"));

        Debug.Log("[ENEMY DEBUG] Animator inicializado: " +
            enemyAnimator.isInitialized);

        Debug.Log("[ENEMY DEBUG] En transicion: " +
            enemyAnimator.IsInTransition(0));

        Debug.Log("[ENEMY DEBUG] Estado idle: " +
            state.IsName("idle"));

        Debug.Log("[ENEMY DEBUG] Estado Walk: " +
            state.IsName("Walk"));

        Debug.Log("[ENEMY DEBUG] Estado attack: " +
            state.IsName("attack"));

        Debug.Log("[ENEMY DEBUG] Tiempo normalizado: " +
            state.normalizedTime);

        // DEBUG: comprobar cual clip esta reproduciendose.
        AnimatorClipInfo[] clips =
            enemyAnimator.GetCurrentAnimatorClipInfo(0);

        foreach (AnimatorClipInfo clip in clips)
        {
            Debug.Log("[ENEMY DEBUG] Clip activo: " +
                clip.clip.name + " | Peso: " + clip.weight);
        }

        // DEBUG: comprobar si un hueso cambia su rotacion.
        if (debugBone != null)
        {
            float rotationChange = Quaternion.Angle(
                lastBoneRotation, debugBone.localRotation
            );

            Debug.Log("[ENEMY DEBUG] Hueso: " +
                debugBone.name);

            Debug.Log("[ENEMY DEBUG] Cambio de rotacion del hueso: " +
                rotationChange);

            lastBoneRotation = debugBone.localRotation;
        }
    }

    // DEBUG: devuelve la ruta completa de un objeto
    // para distinguir los SlendrinaMom con nombres repetidos.
    private string GetObjectPath(Transform objectTransform)
    {
        string path = objectTransform.name;

        while (objectTransform.parent != null)
        {
            objectTransform = objectTransform.parent;
            path = objectTransform.name + "/" + path;
        }

        return path;
    }
}
