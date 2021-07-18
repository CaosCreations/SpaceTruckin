/// <summary>
/// This class holds data about past missions.
/// The fields of this class will be set 
/// during the processing of the mission outcomes.
/// </summary>
[System.Serializable]
public class ArchivedMission
{
    public string MissionName;
    public int CompletionNumber, TotalDamageTaken, TotalDamageReduced, TotalFuelLost;
    public long TotalMoneyEarned, TotalMoneyIncreaseFromLicences;
    public double TotalPilotXpGained, TotalXpIncreaseFromLicences;
    public Pilot Pilot;
    public int PilotLevelAtTimeOfMission;
    public int MissionsCompletedByPilotAtTimeOfMission;
    public Date CompletionDate;
    public MissionModifierOutcome ModifierOutcome;

    /// <summary>Keeps track of whether the mission has been shown in the new day report.</summary>
    public bool HasBeenViewedInReport;

    public ArchivedMission(Mission mission, Pilot pilot, int completionNumber)
    {
        MissionName = mission.Name;
        CompletionNumber = completionNumber;
        Pilot = pilot;
        TotalFuelLost = mission.FuelCost;
        CompletionDate = CalendarManager.Instance.CurrentDate;
    }

    #region Persistence
    public const string FILE_NAME = "ArchivedMissionSaveData";
    public const string FOLDER_NAME = "ArchivedMissionSaveData";
    public static string FilePath
    {
        get => DataUtils.GetSaveFilePath(FOLDER_NAME, FILE_NAME);
    }
    #endregion Persistence
}
