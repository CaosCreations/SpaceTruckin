public interface IMissionOutcome
{
    void Process(ScheduledMission mission, bool isMissionModifierOutcome = false);
}
