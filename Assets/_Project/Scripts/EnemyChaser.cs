
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float detectionDistance = 5f;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float chaseDuration = 5f;

    private bool isChasing;
    private float chaseTimer;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distance <= detectionDistance)
        {
            isChasing = true;
            chaseTimer = chaseDuration;
        }

        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );

            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0f)
                isChasing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isChasing)
            SceneManager.LoadScene("DeathScene");
    }
}
