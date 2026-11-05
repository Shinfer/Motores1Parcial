
using UnityEngine;

public class SwitchInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private Light[] controlledLights;
    [SerializeField] private WorldMessage worldMessage;
    [Header("Settings")]
    [SerializeField] private bool startsOn;
    [SerializeField] private float lightDuration = 0f;
    [SerializeField] private Material materialEncendido;
    [SerializeField] private Material materialApagado;

    public bool IsOn => isOn;

    private bool isOn;
    private float remainingTime; //tiempo restante

    private void Start()
    {
        isOn = startsOn;
        remainingTime = lightDuration;
        foreach (Light light in controlledLights) // Configura el estado inicial de las luces.
        {
            foreach (Light childLight in light.GetComponentsInChildren<Light>(true))
                childLight.enabled = isOn;
        }
        ActualizarMateriales();
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
    private void ActualizarMateriales()
    {
        foreach (Light light in controlledLights)
        {
            MeshRenderer foco = light.GetComponentInChildren<MeshRenderer>(true);

            Material[] materiales = foco.materials;
            materiales[1] = isOn ? materialEncendido : materialApagado;
            foco.materials = materiales;
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
            foreach (Light childLight in light.GetComponentsInChildren<Light>(true))
                childLight.enabled = true;
        }
        worldMessage.ShowMessage("Luces encendidas");
        ActualizarMateriales();
    }
    private void TurnOff()
    {
        isOn = false;
        foreach (Light light in controlledLights)
        {
            foreach (Light childLight in light.GetComponentsInChildren<Light>(true))
                childLight.enabled = false;
        }
        worldMessage.ShowMessage("Luces apagadas");
        ActualizarMateriales();
    }
}