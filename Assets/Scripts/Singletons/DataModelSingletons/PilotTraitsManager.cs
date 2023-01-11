using System;
using System.Linq;
using UnityEngine;

public class PilotTraitsManager : MonoBehaviour
{
    public static PilotTraitsManager Instance { get; private set; }

    [field: SerializeField]
    private PilotSpeciesTraitContainer speciesTraitContainer;
    private PilotSpeciesTrait[] SpeciesTraits => speciesTraitContainer.Elements;

    [field: SerializeField]
    private PilotTraitContainer pilotTraitContainer;
    private PilotTrait[] PilotTraits => pilotTraitContainer.Elements;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
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
    public static float GetMissionChanceEffect(MissionPilotTraitEffect traitEffect)
    {
        return traitEffect.HasPositiveEffect
            ? traitEffect.PilotTrait.PositiveMissionProbabilityEffect
            : -traitEffect.PilotTrait.NegativeMissionProbabilityEffect;
    }

    public static MissionPilotTraitEffect[] GetTraitEffectsForPilot(Pilot pilot,
        MissionPilotTraitEffects traitEffects)
    {
        if (pilot == null || pilot.Traits == null || traitEffects == null)
        {
            Debug.LogError("Null arg(s) when getting pilot trait effects. Unable to get trait effects.");
            return default;
        }

        var speciesTraits = GetSpeciesTraitsBySpecies(pilot.Species);

        return traitEffects.Effects
            .Where(x => pilot.Traits
            .Contains(x?.PilotTrait) || speciesTraits != null && speciesTraits.Contains(x?.PilotTrait))
            .ToArray();
    }

    private static PilotSpeciesTrait[] GetSpeciesTraitsBySpecies(Species species)
    {
        if (Instance == null || Instance.speciesTraitContainer == null || Instance.SpeciesTraits == null)
        {
            Debug.LogError("Pilot species traits were null. Unable to get pilot species traits.");
            return default;
        }
        return Instance.SpeciesTraits.Where(x => x != null && x.Species == species).ToArray();
    }
}