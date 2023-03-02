public class ArchivedMissionModifierOutcome
{
    public MissionModifierOutcome ModifierOutcome { get; set; }
    public ArchivedMissionOutcomeContainer ArchivedMissionOutcomeContainer { get; } = new();
}