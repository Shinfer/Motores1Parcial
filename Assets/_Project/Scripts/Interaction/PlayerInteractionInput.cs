
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionInput : MonoBehaviour
{
    [SerializeField] private RaycastInteractor raycastInteractor;
    public void OnInteract(InputValue value)
    {        
        if (value.isPressed) // Interactua solamente cuando se presiona la tecla.
        {
            raycastInteractor.TryInteract();
        }
    }
}