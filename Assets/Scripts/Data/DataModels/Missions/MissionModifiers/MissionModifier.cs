using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionModifier", menuName = "ScriptableObjects/Missions/MissionModifiers/MissionModifier", order = 1)]
public class MissionModifier : ScriptableObject
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
        // The value that will be compared with the threshold that determines which outcome occurs 
        int attributePoints = GetAttributePointsByType(pilot, DependentAttribute);

        // Choose the outcome with the highest attribute point bracket the Pilot is in 
        MissionModifierOutcome decidedOutcome = PossibleOutcomes
            .OrderByDescending(x => x.AttributePointThreshold)
            .FirstOrDefault(x => attributePoints >= x.AttributePointThreshold);

        return decidedOutcome;
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
