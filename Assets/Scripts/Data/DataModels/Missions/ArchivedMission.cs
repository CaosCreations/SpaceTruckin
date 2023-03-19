using System;

/// <summary>
/// This class holds data about past missions.
/// The fields of this class can be set during the processing of the mission outcomes.
/// </summary>
[Serializable]
public class ArchivedMission
{
    public Mission Mission { get; }
    public Pilot Pilot { get; }
    public int CompletionNumber { get; }

    // Outcome data
    public ArchivedMissionOutcomeContainer ArchivedOutcomeContainer { get; set; } = new();
    public ArchivedMissionModifierOutcome ArchivedModifierOutcome { get; set; } = new();

    /// <summary>
    /// Pilot data at the time the mission was completed, not the current time.
    /// </summary>
    public ArchivedMissionPilotInfo ArchivedPilotInfo { get; set; } = new();

    public Date CompletionDate { get; }

    /// <summary>
    /// Keeps track of whether the mission has been shown in the new day report.
    /// </summary>
    public bool HasBeenViewedInReport { get; set; }

    public ArchivedMission(Mission mission, Pilot pilot, int completionNumber)
    {
        Mission = mission;
        ArchivedModifierOutcome.Modifier = mission.Modifier;
        Pilot = pilot;
        CompletionNumber = completionNumber;
        CompletionDate = CalendarManager.CurrentDate;
    }

    #region Persistence
    public const string FileName = "ArchivedMissionSaveData";
    public const string FolderName = "ArchivedMissionSaveData";
    public static string FilePath
    {
        get => DataUtils.GetSaveFilePath(FolderName, FileName);
    }
    #endregion Persistence
}
