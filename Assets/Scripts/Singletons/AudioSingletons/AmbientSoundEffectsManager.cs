using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneLoadingManager.GetCurrentSceneType() == SceneType.MainStation)
        {
            PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
        }

        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            if (SceneLoadingManager.GetSceneNameByType(SceneType.MainStation) == next.name)
            {
                PlaySoundEffect(AmbientSoundEffect.AsteroidAir);
            }
        };
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
