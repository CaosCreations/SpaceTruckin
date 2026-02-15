using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ActorIntroAnimationData",
    menuName = "ScriptableObjects/DialogueIntroAnimations/ActorIntroAnimationData",
    order = 1)]
public class ActorIntroAnimationData : ScriptableObject
{
    [field: SerializeField]
    public string Key { get; private set; }

    [field: SerializeField]
    public Sprite[] Frames { get; private set; }

    [field: SerializeField]
    public float FrameRate { get; private set; } = 0.1f;

    [field: Header("Dissolve")]
    [field: SerializeField]
    public float DissolveDuration { get; private set; } = 0.5f;

    [field: SerializeField]
    public Color DissolveEdgeColor { get; private set; } = Color.white;

    [field: Header("Scale")]
    [field: SerializeField]
    public float ScalePunchStrength { get; private set; } = 0.05f;

    [field: Header("Shake")]
    [field: SerializeField]
    public CameraShakeSettings IntroShake { get; private set; }

    [field: SerializeField]
    public float HoldDuration { get; private set; } = 0.5f;
}
