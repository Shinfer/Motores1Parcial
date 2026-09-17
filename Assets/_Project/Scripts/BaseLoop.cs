
using UnityEngine;

public class BaseLoop : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform mainCamera;

    [Header("Loop References")]
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform arrivalPoint;
    [SerializeField] private BaseLoop destinationLoop;

    [Header("Settings")]
    [SerializeField] private float lookThreshold = 0.9f;

    private BoxCollider triggerCollider;

    private bool playerInside;
    private bool arrivalLocked;

    private void Start()
    {
        // Obtiene el collider del propio prefab.
        triggerCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // Impide volver a teletransportarse hasta salir
        // completamente del trigger de llegada.
        if (arrivalLocked)
        {
            if (!triggerCollider.bounds.Contains(player.position))
            {
                arrivalLocked = false;
                playerInside = false;
            }

            return;
        }

        // Comprueba la mirada mientras el jugador esta en la zona.
        if (playerInside)
        {
            CheckLookDirection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Registra la entrada del jugador.
        if (other.transform == player)
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Registra la salida y permite futuras activaciones.
        if (other.transform == player)
        {
            playerInside = false;
            arrivalLocked = false;
        }
    }

    private void CheckLookDirection()
    {
        // Calcula la direccion desde la camara hacia el rincon.
        Vector3 direction =
            (lookTarget.position - mainCamera.position).normalized;

        // Compara la direccion de la camara con el objetivo.
        float lookValue = Vector3.Dot(
            mainCamera.forward,
            direction
        );

        if (lookValue >= lookThreshold)
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        // Bloquea el loop de destino antes de mover al jugador.
        destinationLoop.BlockArrival();

        // Desactiva el CharacterController para moverlo.
        characterController.enabled = false;

        player.position = destinationLoop.arrivalPoint.position;

        // Vuelve a activar el controlador.
        characterController.enabled = true;

        // Finaliza la interaccion con el trigger de origen.
        playerInside = false;
    }

    public void BlockArrival()
    {
        // Evita un regreso inmediato al llegar al otro piso.
        arrivalLocked = true;
        playerInside = true;
    }
}