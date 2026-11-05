using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDarkness : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FlashlightController flashlight;
    [SerializeField] private TMP_Text countdownText;

    [Header("Settings")]
    [SerializeField] private float darknessDuration = 10f;
    [SerializeField] private string deathSceneName = "DeathScene";

    private readonly List<LightZone> currentZones = new List<LightZone>();
    private float remainingTime;

    private void Start()
    {
        remainingTime = darknessDuration;
        countdownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        bool illuminated = flashlight.flashlightLight.enabled;

        foreach (LightZone zone in currentZones)
        {
            if (zone.IsLit)
                illuminated = true;
        }

        if (illuminated)
        {
            remainingTime = darknessDuration;
            countdownText.gameObject.SetActive(false);
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            SceneManager.LoadScene(deathSceneName);
            return;
        }

        countdownText.gameObject.SetActive(true);
        countdownText.text = Mathf.Clamp(Mathf.CeilToInt(remainingTime) - 1, 1, 9).ToString();
    }

    public void EnterZone(LightZone zone)
    {
        if (!currentZones.Contains(zone))
            currentZones.Add(zone);
    }

    public void ExitZone(LightZone zone)
    {
        currentZones.Remove(zone);
    }
}
