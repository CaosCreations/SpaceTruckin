using UnityEngine;

public enum InteractionSoundEffect
{
    DoorOpening, DoorClosing, DialogueMessageReceived
}

public class InteractionSoundEffectsManager : SoundEffectsManager<InteractionSoundEffectsManager, InteractionSoundEffect>
{
    #region AudioClips
    [SerializeField] private AudioClip doorOpeningClip;
    [SerializeField] private AudioClip doorClosingClip;
    [SerializeField] private AudioClip dialogueMessageReceivedClip;
    #endregion

    public override AudioClip GetClipBySoundEffectType(InteractionSoundEffect effectType)
    {
        return effectType switch
        {
            InteractionSoundEffect.DoorOpening => doorOpeningClip,
            InteractionSoundEffect.DoorClosing => doorClosingClip,
            InteractionSoundEffect.DialogueMessageReceived => dialogueMessageReceivedClip,
            _ => default,
        };
    }
}
