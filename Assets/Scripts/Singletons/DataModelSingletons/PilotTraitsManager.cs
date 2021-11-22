using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PilotTraitsManager : MonoBehaviour
{
    public static PilotTraitsManager Instance { get; private set; }

    [field: SerializeField]
    public PilotSpeciesTrait[] SpeciesTraits { get; set; } // These are universal 

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
    ///     Gets the aggregate number that will added to the success chance, i.e. the total from all trait effects.
    /// </summary>
    /// <param name="traitEffects"></param>
    public static float GetTotalMissionChanceEffect(Pilot pilot, MissionPilotTraitEffects traitEffects)
    {
        IEnumerable<MissionPilotTraitEffect> traitEffectsForPilot = GetTraitEffectsForPilot(pilot, traitEffects);

        float totalChanceEffect = default;

        foreach (MissionPilotTraitEffect effect in traitEffectsForPilot)
        {
            totalChanceEffect += GetMissionChanceEffect(effect);
        }

        return totalChanceEffect;
    }

    /// <summary>
    ///     Gets the number we will add to the normalised mission probability based on the configuration.
    /// </summary>
    private static float GetMissionChanceEffect(MissionPilotTraitEffect traitEffect)
    {
        return traitEffect.HasPositiveEffect
            ? traitEffect.PilotTrait.PositiveMissionProbabilityEffect
            : -traitEffect.PilotTrait.NegativeMissionProbabilityEffect;
    }

    private static IEnumerable<MissionPilotTraitEffect> GetTraitEffectsForPilot(Pilot pilot,
        MissionPilotTraitEffects traitEffects)
    {
        IEnumerable<PilotSpeciesTrait> speciesTraits = GetSpeciesTraitsBySpecies(pilot.Species);

        return traitEffects.PilotTraitEffects
            .Where(x => pilot.Traits.Contains(x?.PilotTrait) || speciesTraits.Contains(x?.PilotTrait));
    }

    private static IEnumerable<PilotSpeciesTrait> GetSpeciesTraitsBySpecies(Species species)
    {
        return Instance.SpeciesTraits.Where(x => x?.Species == species);
    }
}
