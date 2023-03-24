using System;
using UnityEngine;

[Serializable]
public class NPCAnimationContextByDate
{
    [field: SerializeField]
    public NPCAnimationContext AnimationContext { get; private set; }

    [field: SerializeField]
    public Date Date { get; private set; }
}