using UnityEngine;

public enum UISoundEffectType
{
    Confirm, Back, Error, Select, SelectMechanical
}

public class SoundEffectsManager : AudioManager
{
    // Audio clips for UI 
    [SerializeField] private AudioClip confirmClip;
    [SerializeField] private AudioClip backClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip selectMechancialClip;


}
