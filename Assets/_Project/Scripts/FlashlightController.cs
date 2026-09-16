using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlightLight;

    [Min(0)]
    public int durationSeconds = 300;

    public bool startsOn = false;

    [SerializeField]
    private int remainingSeconds;

    private float remainingTime;
    private bool isOn;

    void Start()
    {
        remainingTime = durationSeconds;
        remainingSeconds = durationSeconds;

        if (startsOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    void Update()
    {
        if (isOn && durationSeconds > 0)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                remainingSeconds = 0;

                TurnOff();
            }
            else
            {
                remainingSeconds = Mathf.CeilToInt(remainingTime);
            }
        }
    }

    public void ToggleFlashlight()
    {
        if (isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    public void TurnOn()
    {
        if (durationSeconds > 0 && remainingSeconds <= 0)
        {
            return;
        }

        isOn = true;
        flashlightLight.enabled = true;
    }

    public void TurnOff()
    {
        isOn = false;
        flashlightLight.enabled = false;
    }
}