using System;
using UnityEngine;

[Serializable]
public class NPCLocation
{
    [field: SerializeField]
    public Vector3 MorningStationPosition { get; private set; }

    [field: SerializeField]
    public Vector3 EveningStationPosition { get; private set; }
}