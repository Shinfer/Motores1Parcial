using UnityEngine;

public class TechBoxInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private WorldMessage worldMessage;
    [SerializeField] private TechBoxInteractable otherBox;
    [SerializeField] private FlashlightController flashlight;
    [SerializeField] private DoorInteractable finalDoor;

    [Header("Settings")]
    [SerializeField] private bool isFirstBox;

    private bool isOn;
    private bool escapeStarted;

    public void Interact(Transform player)
    {
        if (isFirstBox && isOn && otherBox.isOn && !escapeStarted)
        {
            escapeStarted = true;
            finalDoor.Unlock();
            worldMessage.ShowMessage("Energia restablecida. La puerta principal se ha desbloqueado. ¡Es mi oportunidad de escapar!");
            return;
        }

        if (isOn)
        {
            worldMessage.ShowMessage("Estado: Encendido");
            return;
        }

        if (!isFirstBox && !otherBox.isOn)
        {
            worldMessage.ShowMessage("Primero repara la caja 1.");
            return;
        }

        isOn = true;
        flashlight.HalveBattery();
        worldMessage.ShowMessage("Caja reparada. Bateria de linterna reducida a la mitad.");
    }
}
