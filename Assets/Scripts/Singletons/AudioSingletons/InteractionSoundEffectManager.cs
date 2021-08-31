using UnityEngine;

public enum InteractionSoundEffect
{
    DoorOpening, DoorClosing
}

public class InteractionSoundEffectManager : SoundEffectsManager<InteractionSoundEffectManager, InteractionSoundEffect>
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
