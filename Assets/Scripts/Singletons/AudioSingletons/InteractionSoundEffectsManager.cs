using UnityEngine;

public enum InteractionSoundEffect
{
    DoorOpening, DoorClosing
}

public class InteractionSoundEffectsManager : SoundEffectsManager<InteractionSoundEffectsManager, InteractionSoundEffect>
{
    #region AudioClips
    [SerializeField] private AudioClip doorOpeningClip;
    [SerializeField] private AudioClip doorClosingClip;
    #endregion

    public override AudioClip GetClipBySoundEffectType(InteractionSoundEffect effectType)
    {
        return effectType switch
        {
            InteractionSoundEffect.DoorOpening => doorOpeningClip,
            InteractionSoundEffect.DoorClosing => doorClosingClip,
            _ => default,
        };
    }
}
