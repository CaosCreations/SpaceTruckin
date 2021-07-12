using System.Linq;
using UnityEngine;

public class MissionModifier
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public string FlavourText { get; private set; }

    [field: SerializeField]
    public MissionModifierOutcome[] PossibleOutcomes { get; private set; }

    [field: SerializeField]
    public PilotAttributeType DependentAttribute { get; private set; }

    public bool HasRandomOutcomes => PossibleOutcomes == null || PossibleOutcomes.Length <= 0;

    public MissionModifierOutcome GetDecidedOutcome(Pilot pilot)
    {
        int attributePointsToCheck = GetAttributePointsByType(pilot, DependentAttribute);

        // Choose the outcome with the highest attribute point bracket the Pilot is in 
        MissionModifierOutcome outcomeDecided = PossibleOutcomes
            .OrderByDescending(x => x.AttributePointThreshold)
            .FirstOrDefault(x => attributePointsToCheck > x.AttributePointThreshold);

        return outcomeDecided;
    }

    private int GetAttributePointsByType(Pilot pilot, PilotAttributeType attributeType)
    {
        return attributeType switch
        {
            PilotAttributeType.Navigation => pilot.Navigation,
            PilotAttributeType.Savviness => pilot.Savviness,
            _ => default,
        };
    }
}