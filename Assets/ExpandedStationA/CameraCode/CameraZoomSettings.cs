using UnityEngine;

public class CameraZoomSettings
{
    [field: SerializeField]
    public float TargetDistance { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }
}