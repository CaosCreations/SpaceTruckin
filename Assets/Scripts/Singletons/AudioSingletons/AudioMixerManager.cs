using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MixerGroup
{
    Master, Music, SoundEffects
}

public class AudioMixerManager : Singleton<AudioMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;

    #region Mixer Groups
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup soundEffectsGroup;
    #endregion

    #region Mapping
    private static readonly Dictionary<MixerGroup, string> VolumeParameterMap = new Dictionary<MixerGroup, string>
    {
        { MixerGroup.Master, "MasterVolume" },
        { MixerGroup.Music, "MusicVolume" },
        { MixerGroup.SoundEffects, "SoundEffectsVolume" }
    };
    #endregion 

    private void Start()
    {
        LoadMixerPlayerPrefs();
    }

    public float SetMixerGroupVolume(MixerGroup mixerGroup, float value)
    {
        if (!VolumeParameterMap.ContainsKey(mixerGroup))
        {
            Debug.LogError($"MixerGroup parameter {nameof(mixerGroup)} is not mapped");
            return default;
        }

        // Convert normalised value to db 
        float decibels = value.ToDecibels();

        // Set mixer level
        audioMixer.SetFloat(VolumeParameterMap[mixerGroup], decibels);

        // Save to player prefs
        PlayerPrefsManager.SetMixerGroupVolumePref(mixerGroup, value);

        return decibels;
    }

    private void LoadMixerPlayerPrefs()
    {
        SetMixerGroupVolume(MixerGroup.Master,
            PlayerPrefsManager.GetMixerGroupVolumePref(MixerGroup.Master).ToDecibels());

        SetMixerGroupVolume(MixerGroup.Music,
            PlayerPrefsManager.GetMixerGroupVolumePref(MixerGroup.Music).ToDecibels());

        SetMixerGroupVolume(MixerGroup.SoundEffects,
            PlayerPrefsManager.GetMixerGroupVolumePref(MixerGroup.SoundEffects).ToDecibels());
    }
}
