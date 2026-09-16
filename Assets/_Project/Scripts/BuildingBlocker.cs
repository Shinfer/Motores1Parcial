
using UnityEngine;

public class BuildingBlocker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light controlledLight;
    [SerializeField] private GameObject blockerPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentBlocker;

    private void Update()
    {        
        if (controlledLight.enabled && currentBlocker == null) // Si la luz esta encendida, crea el bloqueo.
        {
            currentBlocker = Instantiate(blockerPrefab, spawnPoint.position, spawnPoint.rotation); // Instancia el prefab en el punto configurado.
        }        
        if (!controlledLight.enabled && currentBlocker != null) // Si la luz esta apagada, elimina el bloqueo.
        {
            Destroy(currentBlocker); // Destruye solamente el bloqueo creado por este script.
        }
    }
}