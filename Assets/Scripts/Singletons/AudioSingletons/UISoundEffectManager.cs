using UnityEngine;

public enum UISoundEffect
{
    Confirm, Back, Error, Select, SelectMechanical
}

public class UISoundEffectManager : SoundEffectsManager<UISoundEffectManager, UISoundEffect>
{
    #region AudioClips
    [SerializeField] private AudioClip confirmClip;
    [SerializeField] private AudioClip backClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip selectMechancialClip;
    #endregion

    public override AudioClip GetClipBySoundEffectType(UISoundEffect effectType)
    {
        return effectType switch
        {
            UISoundEffect.Confirm => confirmClip,
            UISoundEffect.Back => backClip,
            UISoundEffect.Error => errorClip,
            UISoundEffect.Select => selectClip,
            UISoundEffect.SelectMechanical => selectMechancialClip,
            _ => default,
        };
    }
}
