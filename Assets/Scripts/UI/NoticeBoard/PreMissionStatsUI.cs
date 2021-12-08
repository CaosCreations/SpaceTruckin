using System.Text;
using UnityEngine;

/// <summary>
/// Used for displaying factors that affect mission success chance/rewards
/// to the player before they assign a pilot to a mission 
/// </summary>
public class PreMissionStatsUI : MonoBehaviour
{
    public static string BuildTraitEffectsStatsText(MissionPilotTraitEffect[] traitEffects)
    {
        StringBuilder builder = new StringBuilder();

        foreach (var effect in traitEffects)
        {
            if (effect.PilotTrait.PositiveMissionProbabilityEffect <= 0
                && effect.PilotTrait.NegativeMissionProbabilityEffect <= 0)
            {
                Debug.LogError($"Pilot trait {effect.PilotTrait.Name} has no positive nor negative effect value.");
                continue;
            }

            builder
                .AppendLine("Trait effects:")
                .AppendLineWithBreaks($"{effect.PilotTrait.Name}: {GetPercentageTraitEffectText(effect)}", 1);
        }

        return builder.ToString();
    }

    public static string BuildTraitEffectsStatsText(Pilot pilot, MissionPilotTraitEffects traitEffectsContainer)
    {
        MissionPilotTraitEffect[] pilotTraitEffects = PilotTraitsManager.GetTraitEffectsForPilot(pilot, traitEffectsContainer);
        return BuildTraitEffectsStatsText(pilotTraitEffects);
    }

    private static string GetPercentageTraitEffectText(MissionPilotTraitEffect effect)
    {
        string percentageText = effect.HasPositiveEffect ? "+" : "-";

        percentageText += effect.HasPositiveEffect
            ? (effect.PilotTrait.PositiveMissionProbabilityEffect * 100).ToString()
            : (effect.PilotTrait.NegativeMissionProbabilityEffect * 100).ToString();

        return percentageText + "%";
    }
}