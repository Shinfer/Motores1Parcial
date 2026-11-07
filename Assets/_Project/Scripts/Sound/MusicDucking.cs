using UnityEngine;

public class MusicDucking : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource voice;

    [SerializeField, Range(0f, 1f)]
    private float duckingFactor = 0.35f;

    [SerializeField, Min(0.01f)]
    private float fadeDown = 0.15f;

    [SerializeField, Min(0.01f)]
    private float fadeUp = 0.6f;
    private float normalVolume;
    private bool initialized;

    void Start()
    {
        if (music == null)
            return;

        normalVolume = music.volume;
        initialized = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized || voice == null)
            return;

        float targetVolume;
        float duration;

        if (voice.isPlaying)
        {
            targetVolume = normalVolume * duckingFactor;
            duration = fadeDown;
        }
        else
        {
            targetVolume = normalVolume;
            duration = fadeUp;
        }

        float difference = normalVolume * (1f - duckingFactor);
        float step = difference * Time.deltaTime
                     / Mathf.Max(0.01f, duration);

        music.volume = Mathf.MoveTowards(
            music.volume,
            targetVolume,
            step
        );
    }

    private void OnDisable()
        {
            if (initialized && music != null)
                music.volume = normalVolume;
        }
    
}
