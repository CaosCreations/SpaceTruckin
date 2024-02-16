using System;
using UnityEngine;

public abstract class SoundEffectsManager<TInstance, TSoundEffect> : AudioManager,
    ISoundEffectManager<TSoundEffect>
    where TInstance : class
    where TSoundEffect : Enum
{
    public static SoundEffectsManager<TInstance, TSoundEffect> Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(TInstance)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public virtual void PlaySoundEffect(TSoundEffect effectType, bool fade = false)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType), fade: fade);
    }

    public abstract AudioClip GetClipBySoundEffectType(TSoundEffect effectType);
}
