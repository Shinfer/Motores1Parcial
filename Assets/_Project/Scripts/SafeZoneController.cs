using UnityEngine;

public class SafeZoneController : MonoBehaviour
{
    [SerializeField] private Light safeZoneLight;
    [SerializeField] private WorldMessage messageController;
    [SerializeField] private float intensityMin = 0.2f;
    [SerializeField] private float intensityMax = 2.0f;
    [SerializeField] private float timeBetween = 0.15f;
    private bool playerInside = false;
    private float timer = 0f;

    void Update()
    {
        // Si el jugador ya entró, no cuenta tiempo ni parpadea
        if (playerInside) return;

        // Suma el tiempo al cronómetro
        timer += Time.deltaTime;

        // 2. ¿Ya pasó el tiempo que configuramos en el inspector?
        if (timer >= timeBetween)
        {
            // 3. Reiniciamos el cronómetro a cero para la siguiente vuelta
            timer = 0f;

            // 4. Cambiamos el estado de la luz
            if (safeZoneLight != null)
            {
                if (safeZoneLight.intensity == intensityMax)
                {
                    safeZoneLight.intensity = intensityMin;
                }
                else
                {
                    safeZoneLight.intensity = intensityMax;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            playerInside = true;
            ActivateSafeZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Al SALIR el jugador, la zona vuelve a ser insegura y el parpadeo regresa
        if (other.CompareTag("Player") && playerInside)
        {
            playerInside = false;
            //Debug.Log("Has salido del refugio. La luz vuelve a fallar.");
        }
    }

    private void ActivateSafeZone()
    {
        if (safeZoneLight != null)
        {
            safeZoneLight.intensity = intensityMax; // Luz fija
        }

        if (messageController != null)
        {
            messageController.ShowMessage("Estas en zona segura");
        }
    }
}
