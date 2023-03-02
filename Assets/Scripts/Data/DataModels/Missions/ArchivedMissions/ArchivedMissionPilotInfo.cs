using System;

/// <summary>
/// Pilot data at the time the mission was completed, not the current time.
/// </summary>
[Serializable]
public class ArchivedMissionPilotInfo
{
    public int LevelAtTimeOfMission { get; set; }
    public double TotalXpAfterMission { get; set; }
    public int MissionsCompletedAtTimeOfMission { get; set; }
}