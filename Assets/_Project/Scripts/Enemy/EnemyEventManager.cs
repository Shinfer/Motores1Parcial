
using UnityEngine;

public class EnemyEventManager : MonoBehaviour
{
    [Header("Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Stairs Event")]
    [SerializeField] private EnemyEventTrigger stairsStart;
    [SerializeField] private EnemyEventTrigger stairsEnd;

    [SerializeField] private Transform[] enemySpawnPoints;

    [SerializeField] private float enemyDuration = 2f;
    [SerializeField] private float enemySpeed = 1f;
    [SerializeField] private float stairsCooldown = 10f;

    [Header("Observer Event")]
    [SerializeField] private EnemyEventTrigger hallTrigger;
    [SerializeField] private Transform observerSpawnPoint;

    [SerializeField] private float observerDuration = 2f;
    [SerializeField] private float observerCooldown = 10f;

    private bool stairsStarted;

    private float stairsCooldownTimer;
    private float observerCooldownTimer;

    private GameObject[] movingEnemies;
    private bool enemiesMoving;
    private float movementTimer;

    private void Update()
    {
        // Actualiza el cooldown del evento de la escalera.
        if (stairsCooldownTimer > 0f)
        {
            stairsCooldownTimer -= Time.deltaTime;
        }

        // Actualiza el cooldown del observador.
        if (observerCooldownTimer > 0f)
        {
            observerCooldownTimer -= Time.deltaTime;
        }

        // Mueve los enemigos mientras dura el evento.
        if (enemiesMoving)
        {
            movementTimer += Time.deltaTime;

            foreach (GameObject enemy in movingEnemies)
            {
                enemy.transform.position +=
                    enemy.transform.forward *
                    enemySpeed *
                    Time.deltaTime;
            }

            // Destruye los enemigos al terminar el evento.
            if (movementTimer >= enemyDuration)
            {
                foreach (GameObject enemy in movingEnemies)
                {
                    Destroy(enemy);
                }

                enemiesMoving = false;
            }
        }
    }

    public void TriggerEntered(EnemyEventTrigger trigger)
    {
        // Registra el inicio de la escalera si no hay cooldown.
        if (trigger == stairsStart && stairsCooldownTimer <= 0f)
        {
            stairsStarted = true;
        }

        // Comprueba el segundo trigger de la escalera.
        if (trigger == stairsEnd && stairsCooldownTimer <= 0f)
        {
            if (stairsStarted)
            {
                SpawnMovingEnemies();

                // Reinicia la secuencia de los dos triggers.
                stairsStarted = false;

                // El cooldown comienza al terminar la aparicion.
                stairsCooldownTimer = enemyDuration + stairsCooldown;
            }
        }

        // Activa el observador si termino su cooldown.
        if (trigger == hallTrigger && observerCooldownTimer <= 0f)
        {
            SpawnObserver();

            // El cooldown comienza al desaparecer el observador.
            observerCooldownTimer = observerDuration + observerCooldown;
        }
    }

    private void SpawnMovingEnemies()
    {
        // Prepara el array para guardar los enemigos.
        movingEnemies = new GameObject[enemySpawnPoints.Length];

        // Instancia un enemigo en cada punto configurado.
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            movingEnemies[i] = Instantiate(
                enemyPrefab,
                enemySpawnPoints[i].position,
                enemySpawnPoints[i].rotation
            );
        }

        // Comienza el movimiento de los enemigos.
        movementTimer = 0f;
        enemiesMoving = true;
    }

    private void SpawnObserver()
    {
        // Instancia el observador en su posicion fija.
        GameObject observer = Instantiate(
            enemyPrefab,
            observerSpawnPoint.position,
            observerSpawnPoint.rotation
        );

        // Lo destruye al terminar su tiempo de aparicion.
        Destroy(observer, observerDuration);
    }
}