using System;

[Serializable]
public abstract class ArchivedMissionOutcome
{
    public Guid Id { get; set; }
    public MissionOutcome Outcome { get; set; }
    public MissionModifier Modifier { get; set; }
    public MissionModifierOutcome ModifierOutcome { get; set; }

    public ArchivedMissionOutcome()
    {
        Id = Guid.NewGuid();
    }

    public ArchivedMissionOutcome(MissionOutcome outcome)
    {
        Id = Guid.NewGuid();
        Outcome = outcome;
    }

    public ArchivedMissionOutcome(MissionOutcome outcome, MissionModifier modifier)
    {
        Id = Guid.NewGuid();
        Outcome = outcome;
        Modifier = modifier;
    }

    public ArchivedMissionOutcome(MissionOutcome outcome, MissionModifier modifier, MissionModifierOutcome modifierOutcome)
    {
        Id = Guid.NewGuid();
        Outcome = outcome;
        Modifier = modifier;
        ModifierOutcome = modifierOutcome;
    }
}