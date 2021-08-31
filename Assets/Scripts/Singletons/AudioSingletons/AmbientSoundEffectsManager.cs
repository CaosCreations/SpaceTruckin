using UnityEngine;

public enum AmbientSoundEffect
{
    AsteroidAir
}

public class AmbientSoundEffectsManager : SoundEffectsManager<AmbientSoundEffectsManager, AmbientSoundEffect>
{
    #region AudioClips
    [SerializeField] private AudioClip asteroidAirClip;
    #endregion

    private void Start()
    {
        PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
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
