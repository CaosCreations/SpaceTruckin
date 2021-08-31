using UnityEngine;

public enum UISoundEffect
{
    Confirm, Back, Error, Select, SelectMechanical
}

public class UISoundEffectManager : AudioManager, ISoundEffectManager<UISoundEffect>
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

    public void PlaySoundEffect(UISoundEffect effectType)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType));
    }

    public AudioClip GetClipBySoundEffectType(UISoundEffect effectType)
    {
        return effectType switch
        {
            UISoundEffect.Confirm => Instance.confirmClip,
            UISoundEffect.Back => Instance.backClip,
            UISoundEffect.Error => Instance.errorClip,
            UISoundEffect.Select => Instance.selectClip,
            UISoundEffect.SelectMechanical => Instance.selectMechancialClip,
            _ => default,
        };
    }

}
