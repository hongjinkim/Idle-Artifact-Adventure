using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System;

public class AudioManager : MonoBehaviour
{ 
     // AudioMixerGroup names
    public static string MusicGroup = "Music";
    public static string SfxGroup = "SFX";

    // parameter suffix
    const string Parameter = "Volume";

    public AudioMixer mainAudioMixer;

    // basic range of UI sound clips
    [Header("UI Sounds")]
    [Tooltip("General button click.")]
    public AudioClip defaultButtonSound;
    [Tooltip("General error sound.")]
    public AudioClip defaultWarningSound;

    //[Header("Game Sounds")]


    void OnEnable()
    {

    }

    void OnDisable()
    {
        
    }

    // plays one-shot audio
    public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
    {
        if (clip == null)
            return;

        GameObject sfxInstance = new GameObject(clip.name);
        sfxInstance.transform.position = sfxPosition;

        AudioSource source = sfxInstance.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        // set the mixer group (e.g. music, sfx, etc.)
        source.outputAudioMixerGroup = GetAudioMixerGroup(SfxGroup);

        // destroy after clip length
        Destroy(sfxInstance, clip.length);
    }

    // return an AudioMixerGroup by name
    public static AudioMixerGroup GetAudioMixerGroup(string groupName)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();

        if (audioManager == null)
            return null;

        if (audioManager.mainAudioMixer == null)
            return null;

        AudioMixerGroup[] groups = audioManager.mainAudioMixer.FindMatchingGroups(groupName);

        foreach (AudioMixerGroup match in groups)
        {
            if (match.ToString() == groupName)
                return match;
        }
        return null;

    }
    // convert linear value between 0 and 1 to decibels
    public static float GetDecibelValue(float linearValue)
    {
        // commonly used for linear to decibel conversion
        float conversionFactor = 20f;

        float decibelValue = (linearValue != 0) ? conversionFactor * Mathf.Log10(linearValue) : -144f;
        return decibelValue;
    }

    // convert decibel value to a range between 0 and 1
    public static float GetLinearValue(float decibelValue)
    {
        float conversionFactor = 20f;

        return Mathf.Pow(10f, decibelValue / conversionFactor);

    }

    // converts linear value between 0 and 1 into decibels and sets AudioMixer level
    public static void SetVolume(string groupName, float linearValue)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
            return;

        float decibelValue = GetDecibelValue(linearValue);

        if (audioManager.mainAudioMixer != null)
        {
            audioManager.mainAudioMixer.SetFloat(groupName, decibelValue);
        }
    }

    // returns a value between 0 and 1 based on the AudioMixer's decibel value
    public static float GetVolume(string groupName)
    {

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
            return 0f;

        float decibelValue = 0f;
        if (audioManager.mainAudioMixer != null)
        {
            audioManager.mainAudioMixer.GetFloat(groupName, out decibelValue);
        }
        return GetLinearValue(decibelValue);
    }

    // convenient methods for playing a range of pre-defined sounds
    public static void PlayDefaultButtonSound()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
            return;

        PlayOneSFX(audioManager.defaultButtonSound, Vector3.zero);
    }

    public static void PlayDefaultWarningSound()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
            return;

        PlayOneSFX(audioManager.defaultWarningSound, Vector3.zero);
    }

    //public static void Play****Sound()
    //{
    //    AudioManager audioManager = FindObjectOfType<AudioManager>();
    //    if (audioManager == null)
    //        return;

    //    PlayOneSFX(audioManager.****Sound, Vector3.zero);
    //}

    // event-handling methods
    void OnSettingsUpdated(PlayerInfo gameData)
    {
        // use the gameData to set the music and sfx volume
        SetVolume(MusicGroup + Parameter, gameData.musicVolume / 100f);
        SetVolume(SfxGroup + Parameter, gameData.sfxVolume / 100f);
    }
}

