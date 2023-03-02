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
    public int CompletionNumber;

    // Outcome data
    public ArchivedMissionOutcomeContainer ArchivedMissionOutcomeContainer = new();
    public ArchivedMissionModifierOutcome ArchivedMissionModifierOutcome = new();

    public MissionEarnings Earnings = new();
    public MissionXpGains XpGains = new();
    public MissionShipChanges ShipChanges = new();
    public MissionModifierChanges MissionMoidifierChanges = new();

    // Pilot
    public ArchivedMissionPilotInfo ArchivedPilotInfo = new();

    // Temp 
    public Pilot Pilot;
    public int PilotLevelAtTimeOfMission;
    public int MissionsCompletedByPilotAtTimeOfMission;

    public Date CompletionDate;
    public MissionModifierOutcome ModifierOutcome;

    /// <summary>Keeps track of whether the mission has been shown in the new day report.</summary>
    public bool HasBeenViewedInReport;

    public ArchivedMission(Mission mission, Pilot pilot, int completionNumber)
    {
        Mission = mission;
        CompletionNumber = completionNumber;
        ArchivedPilotInfo.Pilot = pilot;
        Pilot = pilot;
        ShipChanges.FuelLost = mission.FuelCost;
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
