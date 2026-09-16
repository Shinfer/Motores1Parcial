using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform initSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnPlayer(other.gameObject);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        if (initSpawn == null)
        {
            Debug.Log("Falta asignar el initSpawn");
            return;
        }

        // Captura componentes de movimiento de Unity
        CharacterController controller = player.GetComponent<CharacterController>();

        // Desactiva el componente 
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Ejecuta respawn
        player.transform.position = initSpawn.position;
        player.transform.rotation = initSpawn.rotation;

        // Reactiva controller
        if (controller != null)
        {
            controller.enabled = true;
        }
    }


}
