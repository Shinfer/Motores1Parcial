using UnityEngine;

public class DoorInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private WorldMessage worldMessage;
    [SerializeField] private AudioSource doorSound;

    [Header("Door Settings")]
    [SerializeField] private bool locked = false;
    [SerializeField] private bool invertOpeningDirection = false;
    [SerializeField] private float openingAngle = 100f;
    [SerializeField] private float rotationSpeed = 180f;

    private bool isOpen;
    private bool isMoving;
    private float currentAngle;
    private float targetAngle;

    private void Update()
    {
        if (isMoving)
        {
            currentAngle = Mathf.MoveTowards(
                currentAngle,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );

            doorPivot.localRotation = Quaternion.Euler(0f, currentAngle, 0f);

            if (currentAngle == targetAngle)
                isMoving = false;
        }
    }

    public void Interact(Transform player)
    {
        if (locked)
        {
            worldMessage.ShowMessage("Puerta bloqueada");
            return;
        }

        if (isMoving)
            return;

        if (doorSound != null)
        {
            doorSound.pitch = Random.Range(0.80f, 1.20f); // cambiamos el pitch para que no suene siempre igual
            Debug.Log($"Puerta {name} - Pitch: {doorSound.pitch:F3}", this);
            doorSound.Play();
        }


        if (isOpen)
            CloseDoor();
        else
            OpenDoor(player);
    }

    private void OpenDoor(Transform player)
    {
        Vector3 playerDirection = player.position - transform.position;
        float side = Vector3.Dot(transform.forward, playerDirection);

        if (side >= 0f)
            targetAngle = openingAngle;
        else
            targetAngle = -openingAngle;

        if (invertOpeningDirection)
            targetAngle = -targetAngle;

        isOpen = true;
        isMoving = true;
    }

    public void Unlock()
    {
        locked = false;
    }

    private void CloseDoor()
    {
        targetAngle = 0f;
        isOpen = false;
        isMoving = true;
    }
}
