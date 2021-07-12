using UnityEngine;

public class MissionModifierOutcome : MissionOutcome
{
    // MissionModifierOutcomes can have one or more of any kind of MissionOutcome 
    // e.g. can take damage, find a new pilot, earn money, and earn faction rep 
    [SerializeField] private MissionOutcome[] outcomes;

    [SerializeField] private Sprite sprite;

    [field: SerializeField]
    public int AttributePointThreshold { get; set; }

    public override void Process(ScheduledMission scheduled)
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(scheduled);
        }
    }
}
