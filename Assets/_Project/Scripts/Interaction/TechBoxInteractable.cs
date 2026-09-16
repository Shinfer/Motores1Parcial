
using UnityEngine;

public class TechBoxInteractable : MonoBehaviour, Interactable
{
    [Header("References")]
    [SerializeField] private WorldMessage worldMessage;
    [SerializeField] private TechBoxInteractable secondBox;
    [Header("Settings")]
    [SerializeField] private bool isFirstBox;
    private bool isOn;

    public void Interact(Transform player)
    {        
        if (isFirstBox && !secondBox.isOn) // La primera caja necesita que la segunda este encendida.
        {
            worldMessage.ShowMessage(
                "Estado: Apagado\n" +
                "Conexion: Interrumpida\n\n" +
                "Busca y activa la segunda caja."
            );

            return;
        }        
        isOn = true; // Activa la caja y conserva su estado.
        ShowStatus();
    }
    private void ShowStatus()
    {
        string status;        
        if (isOn) // Determina el estado de encendido.
        {
            status = "Estado: Encendido";
        }
        else
        {
            status = "Estado: Apagado";
        }        
        if (isFirstBox && !isOn) // Agrega el estado de la conexion.
        {
            status += "\nConexion: Interrumpida";
        }
        else
        {
            status += "\nConexion: Establecida";
        }        
        worldMessage.ShowMessage(status); // Muestra el estado mediante WorldMessage.
    }
}