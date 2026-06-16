using UnityEngine;
public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] Sound.Sound_ScriptableObject soundData;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float targetVolume = 1f; // the "full" music volume, fixed

    Coroutine fadeRoutine;

    void Awake()
    {
        musicSource.loop = true;
        musicSource.volume = targetVolume;
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
        float startVolume = musicSource.volume; // wherever it currently is, for a smooth fade-out only

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

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration); // always fades back to the fixed target
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}