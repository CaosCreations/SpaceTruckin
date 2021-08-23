using UnityEngine;

public enum UISoundEffectType
{
    Confirm, Back, Error, Select, SelectMechanical
}

public class SoundEffectsManager : AudioManager
{
    public static SoundEffectsManager Instance { get; private set; }

    // Audio clips for UI 
    [SerializeField] private AudioClip confirmClip;
    [SerializeField] private AudioClip backClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip selectMechancialClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(SoundEffectsManager)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static void PlayUISoundEffect(UISoundEffectType soundEffect)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(soundEffect));
    }

    private static AudioClip GetClipBySoundEffectType(UISoundEffectType soundEffect)
    {
        return soundEffect switch
        {
            UISoundEffectType.Confirm => Instance.confirmClip,
            UISoundEffectType.Back => Instance.backClip,
            UISoundEffectType.Error => Instance.errorClip,
            UISoundEffectType.Select => Instance.selectClip,
            UISoundEffectType.SelectMechanical => Instance.selectMechancialClip,
            _ => default,
        };
    }
}
