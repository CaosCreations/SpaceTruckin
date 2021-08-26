using UnityEngine;

public enum UISoundEffectType
{
    Confirm, Back, Error, Select, SelectMechanical
}

public class UISoundEffectManager : AudioManager, ISoundEffectManager<UISoundEffectType>
{
    public static UISoundEffectManager Instance { get; private set; }

    #region AudioClips
    [SerializeField] private AudioClip confirmClip;
    [SerializeField] private AudioClip backClip;
    [SerializeField] private AudioClip errorClip;
    [SerializeField] private AudioClip selectClip;
    [SerializeField] private AudioClip selectMechancialClip;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(UISoundEffectManager)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlaySoundEffect(UISoundEffectType effectType)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType));
    }

    public AudioClip GetClipBySoundEffectType(UISoundEffectType soundEffect)
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
