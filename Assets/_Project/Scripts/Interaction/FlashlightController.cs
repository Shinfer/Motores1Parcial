using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlightLight;

    [Header("Audio")]
    [SerializeField] private AudioSource toggleSound;

    [Min(0)]
    public int durationSeconds = 300;

    public bool startsOn = false;

    [SerializeField] private int remainingSeconds;

    private float remainingTime;
    private bool isOn;

    public int BatteryPercent => durationSeconds > 0
        ? Mathf.CeilToInt(remainingTime / durationSeconds * 100f)
        : 100;

    private void Start()
    {
        remainingTime = durationSeconds;
        remainingSeconds = durationSeconds;

        if (startsOn)
            TurnOn();
        else
            TurnOff();
    }

    private void Update()
    {
        if (isOn && durationSeconds > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                remainingSeconds = 0;
                TurnOff();
            }
            else
            {
                remainingSeconds = Mathf.CeilToInt(remainingTime);
            }
        }
    }

    public void HalveBattery()
    {
        remainingTime *= 0.5f;
        remainingSeconds = Mathf.CeilToInt(remainingTime);

        if (remainingSeconds <= 0)
            TurnOff();
    }

    public void ToggleFlashlight()
    {
        bool wasOn = isOn;

        if (isOn)
            TurnOff();
        else
            TurnOn();

        if (wasOn != isOn && toggleSound != null)
            toggleSound.Play();
    }

    public void TurnOn()
    {
        if (durationSeconds > 0 && remainingSeconds <= 0)
            return;

        isOn = true;
        flashlightLight.enabled = true;
    }

    public void TurnOff()
    {
        isOn = false;
        flashlightLight.enabled = false;
    }
}
