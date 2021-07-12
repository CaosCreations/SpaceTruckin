using UnityEngine;

[CreateAssetMenu(fileName = "MissionModifierOutcome", menuName = "ScriptableObjects/Missions/MissionModifiers/MissionModifierOutcome", order = 1)]
public class MissionModifierOutcome : ScriptableObject
{
    // MissionModifierOutcomes can have one or more of any kind of MissionOutcome 
    // e.g. can take damage, find a new pilot, earn money, and earn faction rep 
    [field: SerializeField]
    public MissionOutcome[] Outcomes;

    [field: SerializeField]
    public string FlavourText { get; set; }

    [field: SerializeField]
    public Sprite Sprite { get; private set; }

    [field: SerializeField]
    public int AttributePointThreshold { get; private set; }

    public bool HasRandomOutcomes => Outcomes == null || Outcomes.Length <= 0;

    public void Process(ScheduledMission scheduled)
    {
        // Todo: Abstract away archiving code
        // Archive the modifier outcome for displaying in the new day report
        scheduled.Mission.MissionToArchive.ModifierOutcome = this;

        foreach (MissionOutcome outcome in Outcomes)
        {
            outcome.Process(scheduled);
        }
    }
}
