using UnityEngine;

public class BlockerTrigger : MonoBehaviour
{
    [SerializeField] private BuildingBlocker buildingBlocker;
    [SerializeField] private bool activateBlocker;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activateBlocker)
            buildingBlocker.ActivateBlocker();
        else
            buildingBlocker.DeactivateBlocker();
    }
}
