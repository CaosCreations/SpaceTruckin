using System;

[Serializable]
public class ArchivedMissionModifierOutcome
{
    public MissionModifier Modifier { get; set; }
    public MissionModifierOutcome ModifierOutcome { get; set; }
    public ArchivedMissionOutcomeContainer ArchivedMissionOutcomeContainer { get; } = new();
}