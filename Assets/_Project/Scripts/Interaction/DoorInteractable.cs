
using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private WorldMessage worldMessage;
    [Header("Door Settings")]
    [SerializeField] private bool locked = false; //para las puertas bloqueadas
    [SerializeField] private bool invertOpeningDirection = false; //a veces es necesario girar esto de forma prederterminada
    [SerializeField] private float openingAngle = 100f;
    [SerializeField] private float rotationSpeed = 180f;

    private bool isOpen;
    private bool isMoving;
    private float currentAngle;
    private float targetAngle;

    private void Update() 
    {        
        if (isMoving) // La puerta gira solamente cuando esta en movimiento.
        {
            currentAngle = Mathf.MoveTowards(
                currentAngle,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );
            doorPivot.localRotation = Quaternion.Euler(0f, currentAngle, 0f);

            // Termina el movimiento al alcanzar el angulo.
            if (currentAngle == targetAngle)
            {
                isMoving = false;
            }
        }
    }
    public void Interact(Transform player)
    {        
        if (locked) // Una puerta bloqueada muestra un mensaje.
        {
            worldMessage.ShowMessage("Puerta bloqueada");
            return;
        }        
        if (isMoving) // No permite nuevas interacciones mientras gira.
        {
            return;
        }

        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor(player);
        }
    }
    private void OpenDoor(Transform player)
    {        
        Vector3 playerDirection = player.position - transform.position; // Obtiene la direccion desde la puerta hacia el jugador.
        float side = Vector3.Dot(transform.forward, playerDirection); // Determina de que lado esta el jugador.
        if (side >= 0f) // Elige el angulo segun la posicion del jugador.
        {
            targetAngle = openingAngle;
        }
        else
        {
            targetAngle = -openingAngle;
        }
        if (invertOpeningDirection) // Permite corregir puertas con orientacion invertida.
        {
            targetAngle = -targetAngle;
        }
        isOpen = true;
        isMoving = true;
    }

    private void CloseDoor() // Regresa a la posicion original.
    {        
        targetAngle = 0f;
        isOpen = false;
        isMoving = true;
    }
}