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
}
