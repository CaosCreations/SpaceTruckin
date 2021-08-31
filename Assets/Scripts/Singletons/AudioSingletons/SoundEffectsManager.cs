using System;
using UnityEngine;

public abstract class SoundEffectsManager<TInstance, TSoundEffect> : AudioManager,
    ISoundEffectManager<TSoundEffect>
    where TInstance : class
    where TSoundEffect : Enum
{
    public static SoundEffectsManager<TInstance, TSoundEffect> Instance;

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

    public virtual void PlaySoundEffect(TSoundEffect effectType)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType));
    }

    public abstract AudioClip GetClipBySoundEffectType(TSoundEffect effectType);
}
