
using UnityEngine;
using TMPro;

public class WorldMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float displayDuration = 3f;

    private void Start()
    {        
        messageText.gameObject.SetActive(false); // El mensaje comienza oculto.
    }
    public void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);        
        CancelInvoke(nameof(HideMessage));// Reinicia el tiempo si aparece otro mensaje.
        Invoke(nameof(HideMessage), displayDuration);
    }
    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }
}