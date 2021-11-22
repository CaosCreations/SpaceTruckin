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
        var traitEffectsForPilot = GetTraitEffectsForPilot(pilot, traitEffects);

        float totalChanceEffect = default;

        foreach (var traitEffect in traitEffectsForPilot)
        {
            totalChanceEffect += GetMissionChanceEffect(traitEffect);
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

    private static MissionPilotTraitEffect[] GetTraitEffectsForPilot(Pilot pilot,
        MissionPilotTraitEffects traitEffects)
    {
        var speciesTraits = GetSpeciesTraitsBySpecies(pilot.Species);

        return traitEffects.PilotTraitEffects
            .Where(x => pilot.Traits
            .Contains(x?.PilotTrait) || speciesTraits != null && speciesTraits.Contains(x?.PilotTrait))
            .ToArray();
    }

    private static PilotSpeciesTrait[] GetSpeciesTraitsBySpecies(Species species)
    {
        return Instance.SpeciesTraits.Where(x => x?.Species == species).ToArray();
    }
}
