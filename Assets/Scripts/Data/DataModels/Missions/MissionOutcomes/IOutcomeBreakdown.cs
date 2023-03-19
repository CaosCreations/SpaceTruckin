public interface IOutcomeBreakdown
{
    void ArchiveOutcome(ScheduledMission scheduled, bool isMissionModifierOutcome);
    void LogOutcome();
    void ResetOutcome();
}
