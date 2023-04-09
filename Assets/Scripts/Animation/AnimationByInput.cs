using System;
using UnityEngine;

[Serializable]
public class AnimationByInput
{
    [field: SerializeField]
    public KeyCode KeyCode { get; private set; }

    [field: SerializeField]
    public string ButtonName { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public string PlayClipName { get; private set; }

    [field: SerializeField]
    public string ResetClipName { get; private set; }
}