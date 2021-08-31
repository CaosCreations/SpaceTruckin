using UnityEngine;

public enum AmbientSoundEffect
{
    AsteroidAir
}

public class AmbientSoundEffectManager : AudioManager, ISoundEffectManager<AmbientSoundEffect>
{
    public static AmbientSoundEffectManager Instance { get; private set; }

    #region AudioClips
    [SerializeField] private AudioClip asteroidAirClip;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                Debug.LogError($"Audio source in {nameof(AmbientSoundEffectManager)} not found");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Instance.PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
    }

    public void PlaySoundEffect(AmbientSoundEffect effectType)
    {
        Instance.PlayAudioClip(GetClipBySoundEffectType(effectType));
    }

    public AudioClip GetClipBySoundEffectType(AmbientSoundEffect effectType)
    {
        return effectType switch
        {
            AmbientSoundEffect.AsteroidAir => Instance.asteroidAirClip,
            _ => default,
        };
    }
}
