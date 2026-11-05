using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu";

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
