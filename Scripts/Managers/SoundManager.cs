using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip hitSound;

    private bool isBattleSoundsEnabled = false;
    private float volume = 0.1f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(backgroundMusic);
    }
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        musicSource.volume = volume;
        effectsSource.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayHitSound()
    {
        if (isBattleSoundsEnabled)
        {
            effectsSource.PlayOneShot(hitSound);
        }
    }

    public void EnableBattleSounds()
    {
        isBattleSoundsEnabled = true;
    }

    public void DisableBattleSounds()
    {
        isBattleSoundsEnabled = false;
    }
}