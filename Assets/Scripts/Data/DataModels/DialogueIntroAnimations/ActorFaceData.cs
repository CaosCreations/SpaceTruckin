using UnityEngine;

[CreateAssetMenu(
    fileName = "ActorFaceData",
    menuName = "ScriptableObjects/DialogueIntroAnimations/ActorFaceData",
    order = 1)]
public class ActorFaceData : ScriptableObject
{
    [field: SerializeField]
    public string Key { get; private set; }

    [field: SerializeField]
    public Sprite BaseSprite { get; private set; }

    [field: SerializeField]
    public Sprite[] Eyes { get; private set; }

    [field: SerializeField]
    public Sprite[] Mouths { get; private set; }

    [field: SerializeField]
    public Sprite UniversalBlinkSprite { get; private set; }

    [field: SerializeField]
    public Sprite[] EyeBlinks { get; private set; }

    [field: SerializeField]
    public float BlinkIntervalMin { get; private set; } = 2f;

    [field: SerializeField]
    public float BlinkIntervalMax { get; private set; } = 6f;

    [field: SerializeField]
    public float BlinkDuration { get; private set; } = 0.15f;

    public Sprite GetBlinkSprite(int eyeIndex)
    {
        if (EyeBlinks != null && eyeIndex >= 0 && eyeIndex < EyeBlinks.Length && EyeBlinks[eyeIndex] != null)
        {
            return EyeBlinks[eyeIndex];
        }

        return UniversalBlinkSprite;
    }
}
