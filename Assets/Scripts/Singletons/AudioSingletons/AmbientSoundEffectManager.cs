﻿using UnityEngine;

public enum AmbientSoundEffectType
{
    AsteroidAir
}

public class AmbientSoundEffectManager : AudioManager, ISoundEffectManager<AmbientSoundEffectType>
{
    public static AmbientSoundEffectManager Instance { get; private set; }

    #region AudioClips
    [SerializeField] private AudioClip asteroidAirClip;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(AmbientSoundEffectManager)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Instance.PlaySoundEffect(AmbientSoundEffectType.AsteroidAir);
    }

    public void PlaySoundEffect(AmbientSoundEffectType effectType)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType));
    }

    public AudioClip GetClipBySoundEffectType(AmbientSoundEffectType effectType)
    {
        return effectType switch
        {
            AmbientSoundEffectType.AsteroidAir => Instance.asteroidAirClip,
            _ => default,
        };
    }
}
