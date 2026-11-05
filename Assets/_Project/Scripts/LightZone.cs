using UnityEngine;

public class LightZone : MonoBehaviour
{
    [SerializeField] private SwitchInteractable[] switches;
    [SerializeField] private Light[] permanentLights;

    public bool IsLit
    {
        get
        {
            foreach (SwitchInteractable lightSwitch in switches)
            {
                if (lightSwitch.IsOn)
                    return true;
            }

            foreach (Light light in permanentLights)
            {
                if (light.enabled)
                    return true;
            }

            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerDarkness>().EnterZone(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerDarkness>().ExitZone(this);
    }
}
