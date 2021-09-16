using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionsManager : MonoBehaviour, IDataModelManager, ILuaFunctionRegistrar
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    [SerializeField] private MissionOutcomeContainer missionOutcomeContainer;
    [SerializeField] private MissionBonusContainer missionBonusContainer;

    public Mission[] Missions => missionContainer.Elements;
    public MissionOutcome[] Outcomes => missionOutcomeContainer.MissionOutcomes;
    public MissionBonus[] Bonuses => missionBonusContainer.Elements;

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

    private void OnDisable() => UnregisterLuaFunctions();

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Mission.FolderName))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Mission.FolderName);
        }

        LogMissionDataStatus();

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
            DialogueDatabaseManager.AddToActorFondness(
                scheduled.Mission.Customer, scheduled.Mission.FondnessGranted);
        }

        scheduled.Mission.NumberOfCompletions++;

        // Instantiate an Archived Mission object to store the stats of the completed Mission.
        scheduled.Mission.MissionToArchive = new ArchivedMission(
            scheduled.Mission, scheduled.Pilot, scheduled.Mission.NumberOfCompletions);

        // The Archived Mission fields are set throughout the outcome processing.
        ProcessMissionOutcomes(scheduled);

        scheduled.Pilot.MissionsCompleted++;
        scheduled.MissionToArchive.MissionsCompletedByPilotAtTimeOfMission = scheduled.Pilot.MissionsCompleted;

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(scheduled.Mission.MissionToArchive);

        ScheduledMissions.Remove(scheduled);
    }

    private static void ProcessMissionOutcomes(ScheduledMission scheduled)
    {
        // Randomise the Mission's outcomes if flag is set or they are missing. 
        if (scheduled.Mission.HasRandomOutcomes)
        {
            scheduled.Mission.Outcomes = MissionUtils.GetRandomOutcomes(Instance.Outcomes);
        }

        scheduled.Mission.ProcessOutcomes();

        // Some Missions have a MissionModifier, which gives additional outcomes based on attribute conditions.
        if (scheduled.Mission.HasModifier)
        {
            ProcessMissionModifierOutcomes(scheduled);
        }

        // Reset the Mission's Bonus, as it only applies once.
        scheduled.Bonus = null;
    }

    private static void ProcessMissionModifierOutcomes(ScheduledMission scheduled)
    {
        // Decide the outcome from the possible outcomes based on the Pilot's attribute points.
        MissionModifierOutcome modifierOutcome = scheduled.Mission.Modifier.GetDecidedOutcome(scheduled.Pilot);

        if (modifierOutcome != null)
        {
            if (modifierOutcome.HasRandomOutcomes)
            {
                modifierOutcome.Outcomes = MissionUtils.GetRandomOutcomes(Instance.Outcomes);
            }

            modifierOutcome.Process(scheduled);
        }
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

    #region Scheduled Missions
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
    #endregion

    #region Bonuses
    public static MissionBonus GetRandomBonus(bool isPilotSpecific = false)
    {
        var randomBonus = Instance.Bonuses.GetRandomItem();

        if (randomBonus == null)
        {
            Debug.LogError("There are no Bonuses in the container");
            return null;
        }

        randomBonus.RequiredPilot = isPilotSpecific ? PilotsManager.GetRandomHiredPilot() : null;

        return randomBonus;
    }

    /// <summary>
    /// Gets a random Bonus that is not assigned to a Mission (i.e. is null on that Mission field) 
    /// </summary>
    /// <param name="isPilotSpecific"></param>
    /// <returns></returns>
    public static MissionBonus GetRandomBonusNotAttachedToAMission(bool isPilotSpecific = false)
    {
        var availableBonuses = Instance.Bonuses
            .Where(x => !Instance.Missions.Any(
                y => y != null
                && (y.Bonus != null
                || y.Bonus != x)));

        if (availableBonuses.IsNullOrEmpty())
        {
            Debug.LogError("There are no Bonuses in the container that are not already tied to a Mission");
            return null;
        }

        var randomBonus = availableBonuses.GetRandomItem();

        randomBonus.RequiredPilot = isPilotSpecific ? PilotsManager.GetRandomHiredPilot() : null;

        return randomBonus;
    }
    #endregion

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

    public bool HasMissionOfferExpired(string missionName, string customerName)
    {
        Mission missionForCustomer = MissionUtils.GetMissionForCustomer(missionName, customerName);

        if (missionForCustomer != null)
        {
            return missionForCustomer.HasOfferExpired;
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

    #region Lua Function 
    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.MissionCompletedFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => HasMissionBeenCompletedForCustomer(string.Empty, string.Empty)));

        Lua.RegisterFunction(
            DialogueConstants.MissionOfferExpiredFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => HasMissionOfferExpired(string.Empty, string.Empty)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.MissionCompletedFunctionName);
        Lua.UnregisterFunction(DialogueConstants.MissionOfferExpiredFunctionName);
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
        string folderPath = DataUtils.GetSaveFolderPath(Mission.FolderName);

        Debug.Log("Scheduled Mission Json to save: " + json);
        DataUtils.SaveFileAsync(ScheduledMission.FileName, folderPath, json);
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
        string json = await DataUtils.ReadFileAsync(ScheduledMission.FilePath);
        Debug.Log("Scheduled Mission Json to load: " + json);

        ScheduledMissions = JsonHelper.ListFromJson<ScheduledMission>(json);
        if (ScheduledMissions?.Count > 0)
        {
            LogScheduledMissions();
        }
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Mission.FolderName);
    }
    #endregion

    #region Logging
    /// <summary>
    /// Logs if any scriptable object container's are null or empty
    /// </summary>
    private static void LogMissionDataStatus()
    {
        if (Instance.Missions.IsNullOrEmpty())
            Debug.LogError("No Mission data found");

        if (Instance.Outcomes.IsNullOrEmpty())
            Debug.LogError("No MissionOutcome data found");

        if (Instance.Bonuses.IsNullOrEmpty())
            Debug.LogError("No MissionBonus data found");
    }
    #endregion
}
