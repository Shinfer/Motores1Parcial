
using UnityEngine;
using TMPro;

public class NoteInteractable : MonoBehaviour, Interactable
{
    [Header("UI References")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteText;
    [Header("Note Settings")]
    [TextArea(3, 8)]
    [SerializeField] private string noteContent;
    [SerializeField] private float displayDuration = 10f;

    private void Start()
    {        
        HideNote(); // Oculta el panel y el texto al iniciar el juego.
    }

    public void Interact(Transform player)
    {        
        noteText.text = noteContent; // Asigna el contenido de esta nota.        
        notePanel.SetActive(true); // Muestra el fondo y el texto.
        noteText.gameObject.SetActive(true);        
        CancelInvoke(nameof(HideNote)); // Reinicia el temporizador si se interactua nuevamente.
        Invoke(nameof(HideNote), displayDuration);
    }
    private void HideNote()
    {        
        notePanel.SetActive(false); // Oculta ambos elementos de la interfaz.
        noteText.gameObject.SetActive(false);
    }
}