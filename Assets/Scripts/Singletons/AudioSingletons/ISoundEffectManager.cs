using UnityEngine;

public interface ISoundEffectManager<T>
{
    AudioClip GetClipBySoundEffectType(T effectType);
    void PlaySoundEffect(T effectType);
}
