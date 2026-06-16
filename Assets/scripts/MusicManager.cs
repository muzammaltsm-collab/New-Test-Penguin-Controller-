using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] Sound.Sound_ScriptableObject soundData;
    [SerializeField] float fadeDuration = 0.5f;

    Coroutine fadeRoutine;

    void Awake()
    {
        musicSource.loop = true;
    }

    public void PlayMainMenuMusic() => PlayMusic(soundData.MainMenuMusic);
    public void PlayGameplayMusic() => PlayMusic(soundData.GameplayMusic);

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeToClip(clip));
    }

    System.Collections.IEnumerator FadeToClip(AudioClip clip)
    {
        float startVolume = musicSource.volume;

        // fade out
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.clip = clip;
        musicSource.volume = 0f;
        musicSource.Play();

        // fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = startVolume;
    }
}