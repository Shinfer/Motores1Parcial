using UnityEngine;

public class SwitchInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private Light[] controlledLights;
    [SerializeField] private WorldMessage worldMessage;
    [SerializeField] private AudioSource switchSound;
    [SerializeField] private AudioSource[] lightOnSounds;

    [Header("Settings")]
    [SerializeField] private bool startsOn;
    [SerializeField] private float lightDuration = 0f;
    [SerializeField] private Material materialEncendido;
    [SerializeField] private Material materialApagado;

    private bool isOn;
    private float remainingTime;

    public bool IsOn => isOn;

    private void Start()
    {
        isOn = startsOn;
        remainingTime = lightDuration;

        foreach (Light light in controlledLights)
        {
            foreach (Light childLight in light.GetComponentsInChildren<Light>(true))
                childLight.enabled = isOn;
        }

        foreach (AudioSource sound in lightOnSounds)
        {
            if (sound == null) continue;
            sound.loop = true;
            if (isOn) sound.Play();
            else sound.Stop();
        }

        ActualizarMateriales();
    }

    private void Update()
    {
        if (isOn && lightDuration > 0f)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
                TurnOff();
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
        if (switchSound != null)
            switchSound.Play();

        if (isOn) TurnOff();
        else TurnOn();
    }

    private void TurnOn()
    {
        isOn = true;
        remainingTime = lightDuration;

        foreach (Light light in controlledLights)
        {
            foreach (Light childLight in light.GetComponentsInChildren<Light>(true))
                childLight.enabled = true;
        }

        foreach (AudioSource sound in lightOnSounds)
        {
            if (sound != null)
            {
                sound.loop = true;
                sound.Play();
            }
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

        foreach (AudioSource sound in lightOnSounds)
        {
            if (sound != null)
                sound.Stop();
        }

        worldMessage.ShowMessage("Luces apagadas");
        ActualizarMateriales();
    }
}
