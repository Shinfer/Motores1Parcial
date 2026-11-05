using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Slider ambienteSlider;
    [SerializeField] private Slider vocesSlider;

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        fxSlider.value = PlayerPrefs.GetFloat("FXVolume", 1f);
        ambienteSlider.value = PlayerPrefs.GetFloat("AmbienteVolume", 1f);
        vocesSlider.value = PlayerPrefs.GetFloat("VocesVolume", 1f);

        SetMaster(masterSlider.value);
        SetFX(fxSlider.value);
        SetAmbiente(ambienteSlider.value);
        SetVoces(vocesSlider.value);
    }

    public void Jugar()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void SetMaster(float value)
    {
        SetVolume("MasterVolume", value);
    }

    public void SetFX(float value)
    {
        SetVolume("FXVolume", value);
    }

    public void SetAmbiente(float value)
    {
        SetVolume("AmbienteVolume", value);
    }

    public void SetVoces(float value)
    {
        SetVolume("VocesVolume", value);
    }

    private void SetVolume(string parameter, float value)
    {
        mixer.SetFloat(parameter, Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f);
        PlayerPrefs.SetFloat(parameter, value);
    }
}
