using UnityEngine;
using UnityEngine.Audio;

public class SnapshotsZone : MonoBehaviour
{
    [SerializeField] private Transform jugador;
    [SerializeField] private AudioMixerSnapshot exterior;
    [SerializeField] private AudioMixerSnapshot interior;
    [SerializeField, Min(0f)] private float transition = 1.5f;

    private int contacts;

    void Start()
    {
        // La partida comienza en el exterior.
        if (exterior != null)
            exterior.TransitionTo(0f);
    }

    private bool IsPlayer(Collider other)
    {
        // Comprueba que asignamos el jugador en el Inspector.
        if (jugador == null)
            return false;

        // Reconoce al objeto principal del jugador.
        if (other.transform == jugador)
            return true;

        // Reconoce Player y otros hijos del jugador.
        if (other.transform.IsChildOf(jugador))
            return true;

        // El objeto que entró no pertenece al jugador.
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other))
            return;

        contacts++;

        // Activa Interior al entrar al primer volumen.
        if (contacts == 1 && interior != null)
            interior.TransitionTo(transition);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other))
            return;

        contacts = Mathf.Max(0, contacts - 1);

        // Activa Exterior cuando abandona todos los volúmenes.
        if (contacts == 0 && exterior != null)
            exterior.TransitionTo(transition);
    }
}
