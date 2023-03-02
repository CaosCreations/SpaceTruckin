using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArchivedMissionsManager : MonoBehaviour, IDataModelManager
{
    public static ArchivedMissionsManager Instance { get; private set; }
    public List<ArchivedMission> ArchivedMissions { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Init()
    {
        ArchivedMissions = new List<ArchivedMission>();
    }

    public static void AddToArchive(ArchivedMission archivedMission)
    {
        Instance.ArchivedMissions.Add(archivedMission);
    }

    public static List<ArchivedMission> GetMissionsCompletedYesterday()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null
            && x.CompletionDate.ToDays() == CalendarManager.Instance.CurrentDate.ToDays() - 1)
            .ToList();
    }

    public static bool ThereAreMissionsToReport()
    {
        return Instance.ArchivedMissions.Any(x => x != null && !x.HasBeenViewedInReport);
    }

    public static List<ArchivedMission> GetMissionsToAppearInReport()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null && !x.HasBeenViewedInReport)
            .ToList();
    }

    public static ArchivedMission GetMostRecentMissionByPilot(Pilot pilot)
    {
        return Instance.ArchivedMissions
            .OrderByDescending(x => x?.CompletionDate)
            .FirstOrDefault(x => x?.Pilot == pilot);
    }

    public static IEnumerable<ArchivedMission> GetArchivedMissionsByPilot(Pilot pilot)
    {
        return Instance.ArchivedMissions.Where(x => x != null && x.Pilot == pilot);
    }

    public static ArchivedMissionViewModel GetArchivedMissionViewModel(ArchivedMission archivedMission)
    {
        // Money 
        long totalBaseMoneyEarned = default;
        long totalEarnedFromLicences = default;
        long totalEarnedFromBonuses = default;

        foreach (var moneyOutcome in archivedMission.ArchivedOutcomeContainer.MoneyOutcomes)
        {
            totalBaseMoneyEarned += (long)moneyOutcome.TotalEarnings;
            totalEarnedFromLicences += (long)moneyOutcome.LicencesEarnings;
            totalEarnedFromBonuses += (long)moneyOutcome.BonusesEarnings;
        }

        var missionEarnings = new MissionEarnings(totalBaseMoneyEarned, totalEarnedFromLicences, totalEarnedFromBonuses);

        // XP 
        double totalBaseXpGained = default;
        double totalLicencesXpGained = default;
        double totalBonusesXpGained = default;

        foreach (var xpOutcome in archivedMission.ArchivedOutcomeContainer.ArchivedPilotXpOutcomes)
        {
            totalBaseXpGained += xpOutcome.BaseXpGain;
            totalLicencesXpGained += xpOutcome.LicencesXpGain;
            totalBonusesXpGained += xpOutcome.BonusesXpGain;
        }

        var missionXpGains = new MissionXpGains(totalBaseXpGained, totalLicencesXpGained, totalBonusesXpGained);

        // Damage 
        int totalDamageTaken = default;
        int totalDamageReduced = default;

        foreach (var damageOutcome in archivedMission.ArchivedOutcomeContainer.ArchivedShipDamageOutcomes)
        {
            totalDamageTaken += damageOutcome.DamageTaken;
            totalDamageReduced+= damageOutcome.DamageReduced;
        }

        var damageType = (ShipDamageType)archivedMission.ArchivedOutcomeContainer.ArchivedShipDamageOutcomes.FirstOrDefault()?.DamageType;
        var shipChanges = new MissionShipChanges(totalDamageTaken, totalDamageReduced, archivedMission.Mission.FuelCost, damageType);

        return new ArchivedMissionViewModel(
            archivedMission.Mission, archivedMission.Pilot, archivedMission.ArchivedPilotInfo, missionEarnings, missionXpGains, shipChanges);
    }

    #region Persistence
    public async void LoadDataAsync()
    {
        if (!DataUtils.SaveFolderExists(ArchivedMission.FolderName))
        {
            DataUtils.CreateSaveFolder(ArchivedMission.FolderName);
            return;
        }

        string json = await DataUtils.ReadFileAsync(ArchivedMission.FilePath);
        ArchivedMissions = JsonHelper.ListFromJson<ArchivedMission>(json);
    }

    public void SaveData()
    {
        string json = JsonHelper.ListToJson(ArchivedMissions);
        string folderPath = DataUtils.GetSaveFolderPath(ArchivedMission.FolderName);
        DataUtils.SaveFileAsync(ArchivedMission.FileName, folderPath, json);
    }

    public void DeleteData() => DataUtils.RecursivelyDeleteSaveData(ArchivedMission.FolderName);
    #endregion
}
