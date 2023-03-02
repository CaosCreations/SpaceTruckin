using System;
/// <summary>
/// This class holds data about past missions.
/// The fields of this class will be set 
/// during the processing of the mission outcomes.
/// </summary>
[Serializable]
public class ArchivedMission
{
    public string MissionName;
    public int CompletionNumber, TotalDamageTaken, TotalDamageReduced, TotalFuelLost;
    public ShipDamageType DamageType;

    // Money
    public long TotalMoneyEarned, TotalAdditionalMoneyEarned;
    public double TotalMoneyIncreaseFromLicences, TotalMoneyIncreaseFromBonuses;

    // XP 
    public double TotalPilotXpGained, TotalXpIncreaseFromLicences, TotalXpIncreaseFromBonuses,
        TotalAdditionalXpGained,
        TotalXpAfterMission;

    public MissionEarnings Earnings = new();
    public MissionXpGains XpGains = new();
    public MissionShipChanges ShipChanges = new();

    // Pilot
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
