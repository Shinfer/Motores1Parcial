using UnityEngine;

public class BuildingBlocker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject blockerPrefab;
    [SerializeField] private Transform spawnPoint;
    private GameObject currentBlocker;
    private bool permanentlyUnlocked;

    public void ActivateBlocker()
    {
        if (permanentlyUnlocked || currentBlocker != null)
            return;

        currentBlocker = Instantiate(blockerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    public void DeactivateBlocker()
    {
        if (currentBlocker == null)
            return;

        Destroy(currentBlocker);
        currentBlocker = null;
    }
    public void UnlockBlocker()
    {
        permanentlyUnlocked = true;
        DeactivateBlocker();
    }
}