using UnityEngine;

public class PilotTraitsManager : MonoBehaviour
{
    public static PilotTraitsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    ///     Gets the aggregate coefficient, i.e. the total from all trait effects.
    /// </summary>
    /// <param name="traitEffects"></param>
    /// <returns></returns>
    public static float GetTotalMissionChanceEffect(MissionPilotTraitEffects traitEffects)
    {
        float totalChanceEffect = default;

        foreach (MissionPilotTraitEffect effect in traitEffects.PilotTraitEffects)
        {
            totalChanceEffect += GetMissionChanceEffect(effect);
        }

        return totalChanceEffect;
    }

    /// <summary>
    ///     Gets the number we multiple the mission probability by based on the configuration.
    /// </summary>
    public static float GetMissionChanceEffect(MissionPilotTraitEffect traitEffect)
    {
        return traitEffect.HasPositiveEffect
            ? traitEffect.PilotTrait.PositiveMissionProbabilityEffect
            : -traitEffect.PilotTrait.NegativeMissionProbabilityEffect;
    }
}
