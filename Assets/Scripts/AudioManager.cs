using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip peaceful, tense;

    void Awake() { if (Instance == null) Instance = this; else Destroy(gameObject); }

    public void PlayMusicPeaceful() { musicSource.clip = peaceful; musicSource.loop = true; musicSource.Play(); }
    public void PlayMusicTense() { musicSource.clip = tense; musicSource.loop = true; musicSource.Play(); }
    public void PlaySFX(AudioClip clip) { sfxSource.PlayOneShot(clip); }
    public void PlaySFX(string name) {
        // Implement dictionary if desired. Or use switch/case.
    }
}
