using System;
using UnityEngine;

[Serializable]
public class CameraZoomSettings
{
    [field: SerializeField]
    public float TargetDistance { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public bool ResetAfter { get; private set; }
}