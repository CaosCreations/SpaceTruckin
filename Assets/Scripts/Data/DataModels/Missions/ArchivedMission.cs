using System;
/// <summary>
/// This class holds data about past missions.
/// The fields of this class will be set 
/// during the processing of the mission outcomes.
/// </summary>
[Serializable]
public class ArchivedMission
{
    public Mission Mission;
    public Pilot Pilot;
    public int CompletionNumber;

    // Outcome data
    public ArchivedMissionOutcomeContainer ArchivedOutcomeContainer = new();
    public ArchivedMissionModifierOutcome ArchivedModifierOutcome = new();

    // Pilot data at the time the mission was completed, not the current time
    public ArchivedMissionPilotInfo ArchivedPilotInfo = new();

    public Date CompletionDate;

    /// <summary>Keeps track of whether the mission has been shown in the new day report.</summary>
    public bool HasBeenViewedInReport;

    public ArchivedMission(Mission mission, Pilot pilot, int completionNumber)
    {
        Mission = mission;
        ArchivedModifierOutcome.Modifier = mission.Modifier;
        Pilot = pilot;
        CompletionNumber = completionNumber;
        CompletionDate = CalendarManager.Instance.CurrentDate;
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
