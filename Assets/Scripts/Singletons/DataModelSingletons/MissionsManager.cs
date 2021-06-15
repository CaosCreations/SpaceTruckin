using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionsManager : MonoBehaviour, IDataModelManager, ILuaFunctionRegistrar
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    [SerializeField] private MissionOutcomeContainer missionOutcomeContainer;
    public Mission[] Missions { get => missionContainer.missions; }
    public MissionOutcome[] Outcomes { get => missionOutcomeContainer.missionOutcomes; }
    public static List<ScheduledMission> ScheduledMissions;

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

    private void OnDisable()
    {
        UnregisterLuaFunctions();
    }

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Mission.FOLDER_NAME);
        }

        if (Missions?.Length <= 0)
        {
            Debug.LogError("No mission data found");
        }

        ScheduledMissions = new List<ScheduledMission>();

        UnlockMissions();

        RegisterLuaFunctions();

        CalendarManager.OnEndOfDay += UpdateMissionSchedule;
        CalendarManager.OnEndOfDay += ApplyOfferExpiryConsequences;
    }

    /// <summary>
    /// Missions are 'selectable' if they have been taken from the notice board
    /// but a pilot is not currently assigned to them
    /// </summary>
    /// <returns></returns>
    public static List<Mission> GetSelectableMissions()
    {
        return Instance.Missions
            .Where(x => x.IsAvailableForScheduling)
            .ToList();
    }

    public static void UpdateMissionSchedule()
    {
        // Reset yesterday's Missions, so today's will take their place. 
        ArchivedMissionsManager.ResetMissionsCompletedYesterday();

        foreach (ScheduledMission scheduled in ScheduledMissions.ToList())
        {
            if (scheduled?.Mission == null || scheduled?.Pilot == null)
            {
                // Outcome processing depends on Missions and Pilots 
                RemoveScheduledMission(scheduled);
                continue;
            }

            if (scheduled.Mission.IsInProgress())
            {
                scheduled.Mission.DaysLeftToComplete--;

                // We just finished the Mission
                if (!scheduled.Mission.IsInProgress())
                {
                    CompleteMission(scheduled);
                    RemoveScheduledMission(scheduled);
                }
            }
        }
    }

    private static void CompleteMission(ScheduledMission scheduled)
    {
        if (scheduled.Mission.NumberOfCompletions <= 0)
        {
            // Send a thank you email on first completion of the Mission.
            if (scheduled.Mission.ThankYouMessage != null)
            {
                scheduled.Mission.ThankYouMessage.IsUnlocked = true;
            }

            // Improve relationship with the client of the mission.
            DialogueDatabaseManager.AddToActorFondness(scheduled.Mission.Customer, scheduled.Mission.FondnessGranted);
        }

        scheduled.Mission.NumberOfCompletions++;

        // Instantiate an Archived Mission object to store the stats of the completed Mission.
        scheduled.Mission.MissionToArchive = new ArchivedMission(scheduled.Mission, scheduled.Pilot, scheduled.Mission.NumberOfCompletions);

        scheduled.Pilot.MissionsCompleted++;
        scheduled.Mission.MissionToArchive.MissionsCompletedByPilotAtTimeOfMission = scheduled.Pilot.MissionsCompleted;

        // Randomise the Mission's outcomes if flag is set or they are missing. 
        if (scheduled.Mission.HasRandomOutcomes)
        {
            AssignRandomOutcomes(scheduled.Mission);
        }

        // We will set the Archived Mission fields throughout the outcome processing. 
        scheduled.Mission.ProcessOutcomes();

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(scheduled.Mission.MissionToArchive);

        ScheduledMissions.Remove(scheduled);
    }

    private static void UnlockMissions()
    {
        foreach (Mission mission in Instance.Missions)
        {
            if (!mission.HasBeenUnlocked
                && mission.UnlockCondition == MissionUnlockCondition.TotalMoney)
            {
                // Unlock missions that require money and subscribe to event to keep them updated.
                mission.UnlockIfConditionMet();
                PlayerManager.OnFinancialTransaction += mission.UnlockIfConditionMet;
            }
        }
    }

    private static void AssignRandomOutcomes(Mission mission)
    {
        var randomOutcomes = new List<MissionOutcome>();
        Instance.Outcomes.Shuffle();

        MoneyOutcome moneyOutcome = MissionUtils.GetOutcomeByType<MoneyOutcome>(Instance.Outcomes);
        PilotXpOutcome pilotXpOutcome = MissionUtils.GetOutcomeByType<PilotXpOutcome>(Instance.Outcomes);
        OmenOutcome omenOutcome = MissionUtils.GetOutcomeByType<OmenOutcome>(Instance.Outcomes);
        ShipDamageOutcome shipDamageOutcome = MissionUtils.GetOutcomeByType<ShipDamageOutcome>(Instance.Outcomes);

        if (moneyOutcome != null) randomOutcomes.Add(moneyOutcome);
        if (pilotXpOutcome != null) randomOutcomes.Add(pilotXpOutcome);
        if (omenOutcome != null) randomOutcomes.Add(omenOutcome);
        if (shipDamageOutcome != null) randomOutcomes.Add(shipDamageOutcome);

        mission.Outcomes = randomOutcomes.ToArray();
    }

    public static ScheduledMission GetScheduledMission(Mission mission)
    {
        return ScheduledMissions.FirstOrDefault(x => x.Mission == mission);
    }

    public static ScheduledMission GetScheduledMission(Pilot pilot)
    {
        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if (scheduled?.Pilot != null && scheduled?.Pilot == pilot)
            {
                return scheduled;
            }
        }
        return null;
    }

    public static ScheduledMission GetScheduledMission(Ship ship)
    {
        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if (scheduled?.Pilot != null
                && scheduled?.Pilot.Ship != null
                && scheduled?.Pilot.Ship == ship)
            {
                return scheduled;
            }
        }
        return null;
    }

    public static ScheduledMission GetScheduledMission(int node)
    {
        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if (scheduled?.Mission != null
                && scheduled?.Pilot != null
                && scheduled?.Pilot.Ship != null
                && scheduled.Pilot.Ship == HangarManager.GetShipByNode(node))
            {
                return scheduled;
            }
        }
        return null;
    }

    public static void AddOrUpdateScheduledMission(Pilot pilot, Mission mission)
    {
        ScheduledMission scheduledMission = GetScheduledMission(mission);
        if (scheduledMission != null)
        {
            scheduledMission.Pilot = pilot;
        }
        else
        {
            scheduledMission = new ScheduledMission { Mission = mission, Pilot = pilot };
            ScheduledMissions.Add(scheduledMission);
        }
        Debug.Log("Scheduled Mission added/updated: " + GetScheduledMissionString(scheduledMission));
    }

    public static void RemoveScheduledMission(ScheduledMission scheduledMission)
    {
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(scheduledMission);
            Debug.Log("Scheduled Mission removed: " + GetScheduledMissionString(scheduledMission));
        }
    }

    public static void RemoveScheduledMission(Mission mission)
    {
        ScheduledMission scheduledMission = GetScheduledMission(mission);
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(scheduledMission);
            Debug.Log("Scheduled Mission removed: " + GetScheduledMissionString(scheduledMission));
        }
    }

    public static Mission GetMission(Pilot pilot)
    {
        return GetScheduledMission(pilot)?.Mission;
    }

    public static Mission GetMission(Ship ship)
    {
        return GetScheduledMission(ship)?.Mission;
    }

    private static string GetScheduledMissionString(ScheduledMission scheduled)
    {
        if (scheduled?.Mission != null && scheduled?.Pilot != null)
        {
            return $"{scheduled.Mission.Name} (Mission), {scheduled.Pilot.Name} (Pilot)";
        }
        return string.Empty;
    }

    private static void LogScheduledMissions()
    {
        Debug.Log("Currently scheduled missions:\n");
        ScheduledMissions.ForEach(x =>
        {
            string stringRepresentation = GetScheduledMissionString(x);
            if (!string.IsNullOrEmpty(stringRepresentation
                .RemoveAllWhitespace()
                .RemoveCharacter(',')))
            {
                Debug.Log(GetScheduledMissionString(x));
            }
        });
    }

    #region Dialogue Integration
    public bool HasMissionBeenCompletedForCustomer(string missionName, string customerName)
    {
        Mission missionForCustomer = MissionUtils.GetMissionForCustomer(missionName, customerName);

        if (missionForCustomer != null)
        {
            return missionForCustomer.NumberOfCompletions > 0;
        }

        return false;
    }

    private static void ApplyOfferExpiryConsequences()
    {
        foreach (Mission mission in Instance.Missions)
        {
            if (mission != null
                && mission.OfferTimeLimitInDays > 0
                && !mission.OfferExpiryConsequencesApplied
                && mission.HasOfferExpired)
            {
                // Deduct relationship points from the customer as the deadline has elapsed.
                DialogueDatabaseManager.AddToActorFondness(
                    mission.Customer, -mission.OfferExpiryFondnessDeduction);

                // Prevent repeat deductions 
                mission.OfferExpiryConsequencesApplied = true;
            }
        }
    }
    #endregion

    #region Lua Function Registration
    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.MissionCompletedFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => HasMissionBeenCompletedForCustomer(string.Empty, string.Empty)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.MissionCompletedFunctionName);
    }
    #endregion

    #region Persistence
    public void SaveData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            mission.SaveData();
        }
        SaveScheduledMissionData();
    }

    private void SaveScheduledMissionData()
    {
        string json = JsonHelper.ListToJson(ScheduledMissions);
        string folderPath = DataUtils.GetSaveFolderPath(Mission.FOLDER_NAME);
        Debug.Log("Scheduled Mission Json to save: " + json);
        DataUtils.SaveFileAsync(ScheduledMission.FILE_NAME, folderPath, json);
    }

    public async void LoadDataAsync()
    {
        foreach (Mission mission in Instance.Missions)
        {
            await mission.LoadDataAsync();
        }
        LoadScheduledMissionData();
    }

    private async void LoadScheduledMissionData()
    {
        string json = await DataUtils.ReadFileAsync(ScheduledMission.FILE_PATH);
        Debug.Log("Scheduled Mission Json to load: " + json);

        ScheduledMissions = JsonHelper.ListFromJson<ScheduledMission>(json);
        if (ScheduledMissions?.Count > 0)
        {
            LogScheduledMissions();
        }
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
    #endregion
}
