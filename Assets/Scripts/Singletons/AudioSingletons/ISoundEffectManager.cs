using System;
using UnityEngine;

public interface ISoundEffectManager<T> where T : Enum
{
    AudioClip GetClipBySoundEffectType(T effectType);
    void PlaySoundEffect(T effectType);
}
