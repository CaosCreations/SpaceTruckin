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

    public void SetMixerGroupVolume(MixerGroup mixerGroup, float value)
    {
        if (!VolumeParameterMap.ContainsKey(mixerGroup))
        {
            Debug.LogError($"MixerGroup parameter {nameof(mixerGroup)} is not mapped");
        }
        else
        {
            audioMixer.SetFloat(VolumeParameterMap[mixerGroup], value.ToDecibels());
        }
    }
}
