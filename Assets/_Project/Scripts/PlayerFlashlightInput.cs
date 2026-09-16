using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlashlightInput : MonoBehaviour
{
    public FlashlightController flashlight;

    public void OnFlashlight(InputValue value)
    {
        if (value.isPressed)
        {
            flashlight.ToggleFlashlight();
        }
    }
}