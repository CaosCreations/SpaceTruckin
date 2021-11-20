using System;
using UnityEngine;

[Serializable]
public class MissionPilotTraitEffect
{

    [field: SerializeField]
    public PilotTrait PilotTrait { get; set; }

    [field: SerializeField]
    public bool HasPositiveEffect { get; set; }
}
