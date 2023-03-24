using System;
using UnityEngine;

[Serializable]
public class NPCAnimationContext
{
    [field: SerializeField]
    public string MorningParameterName { get; private set; }

    [field: SerializeField]
    public string EveningParameterName { get; private set; }
}