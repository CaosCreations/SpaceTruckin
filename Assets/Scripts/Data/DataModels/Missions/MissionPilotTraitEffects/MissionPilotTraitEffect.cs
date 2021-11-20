using System;
using UnityEngine;

/// <summary>
///     A model of a PilotTrait that affects a particular mission,
///     and whether it has a positive or negative effect on it.
/// </summary>
[Serializable]
public class MissionPilotTraitEffect
{
    [field: SerializeField]
    public PilotTrait PilotTrait { get; set; }

    [field: SerializeField]
    public bool HasPositiveEffect { get; set; }
}
