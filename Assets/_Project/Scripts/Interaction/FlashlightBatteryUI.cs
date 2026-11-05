using TMPro;
using UnityEngine;

public class FlashlightBatteryUI : MonoBehaviour
{
    [SerializeField] private FlashlightController flashlight;
    [SerializeField] private TMP_Text batteryText;

    private void Update()
    {
        batteryText.text = "Batería de la linterna: " + flashlight.BatteryPercent + "%";
    }
}