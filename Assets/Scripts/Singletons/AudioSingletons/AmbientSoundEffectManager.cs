using UnityEngine;

public enum AmbientSoundEffect
{
    AsteroidAir
}

public class AmbientSoundEffectManager : SoundEffectsManager<AmbientSoundEffectManager, AmbientSoundEffect>
{
    #region AudioClips
    [SerializeField] private AudioClip asteroidAirClip;
    #endregion

    private void Start()
    {
        Instance.PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
    }

    public override AudioClip GetClipBySoundEffectType(AmbientSoundEffect effectType)
    {
        return effectType switch
        {
            AmbientSoundEffect.AsteroidAir => asteroidAirClip,
            _ => default,
        };
    }
}
