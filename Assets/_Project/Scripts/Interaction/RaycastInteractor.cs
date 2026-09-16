
using UnityEngine;

public class RaycastInteractor : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform player;

    private void Update()
    {
        // Dibuja el Raycast en verde constantemente.
        Debug.DrawRay(
            transform.position,
            transform.forward * interactionDistance,
            Color.green
        );
    }

    public void TryInteract()
    {
        Debug.Log("Tecla E presionada");
        RaycastHit hit;

        // Detecta objetos en la Layer Interactable.
        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            interactionDistance,
            interactableLayer,
            QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Objeto detectado: " + hit.collider.name);

            Interactable interactable =
                hit.collider.GetComponentInParent<Interactable>();

            interactable.Interact(player);
        }
        else
        {
            Debug.Log("Raycast no detecto ningun objeto");
        }
    }
}