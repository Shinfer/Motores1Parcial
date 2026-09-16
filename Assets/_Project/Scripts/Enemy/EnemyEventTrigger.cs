
using UnityEngine;

public class EnemyEventTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyEventManager eventManager;
    [SerializeField] private Transform player;

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba que el objeto que entro sea el jugador.
        if (other.transform == player)
        {
            // Avisa al manager que se activo este trigger.
            eventManager.TriggerEntered(this);
        }
    }
}