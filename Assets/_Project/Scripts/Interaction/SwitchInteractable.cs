
using UnityEngine;

public class SwitchInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private Light[] controlledLights;
    [SerializeField] private WorldMessage worldMessage;
    [Header("Settings")]
    [SerializeField] private bool startsOn;
    [SerializeField] private float lightDuration = 0f;

    private bool isOn;
    private float remainingTime; //tiempo restante

    private void Start()
    {
        isOn = startsOn;
        remainingTime = lightDuration;        
        foreach (Light light in controlledLights) // Configura el estado inicial de las luces.
        {
            light.enabled = isOn;
        }
    }
    private void Update()
    {        
        if (isOn && lightDuration > 0f) // Cero significa que las luces no tienen temporizador.
        {
            remainingTime -= Time.deltaTime; //contador
            if (remainingTime <= 0f)
            {
                TurnOff();
            }
        }
    }
    public void Interact(Transform player)
    {        
        if (isOn) // Alterna entre encendido y apagado.
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }
    private void TurnOn()
    {
        isOn = true;        
        remainingTime = lightDuration; // Reinicia el temporizador en cada encendido.
        foreach (Light light in controlledLights)
        {
            light.enabled = true;
        }
        worldMessage.ShowMessage("Luces encendidas");
    }
    private void TurnOff()
    {
        isOn = false;
        foreach (Light light in controlledLights)
        {
            light.enabled = false;
        }
        worldMessage.ShowMessage("Luces apagadas");
    }
}