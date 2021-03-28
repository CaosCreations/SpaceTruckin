
using UnityEngine;

public class MissionModifierOutcome : MissionOutcome
{
    // Mission modifiers can have one or more of any kind of outcome 
    // e.g. can take damage, find a new pilot, earn money, and earn faction rep 
    public MissionOutcome[] outcomes;

    public Sprite sprite;

    public override void Process(ScheduledMission mission)
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(mission);
        }
    }
}
