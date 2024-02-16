﻿using System;
using UnityEngine;

public interface ISoundEffectManager<T> where T : Enum
{
    abstract AudioClip GetClipBySoundEffectType(T effectType);
    void PlaySoundEffect(T effectType, bool fade = false);
}
