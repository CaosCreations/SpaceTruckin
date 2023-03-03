using System;

[Serializable]
public abstract class ArchivedMissionOutcome
{
    public Guid Id { get; set; }
    public MissionOutcome Outcome { get; set; }

    public ArchivedMissionOutcome()
    {
        Id = Guid.NewGuid();
    }

    public ArchivedMissionOutcome(MissionOutcome outcome)
    {
        Id = Guid.NewGuid();
        Outcome = outcome;
    }
}