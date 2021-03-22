using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
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

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Mission.FOLDER_NAME);
            //DataUtils.CreateSaveFolder(ScheduledMission.FOLDER_NAME);
        }

        if (Missions?.Length <= 0)
        {
            Debug.LogError("No mission data found");
        }
        ScheduledMissions = new List<ScheduledMission>();
    }

    /// <summary>
    /// Missions are 'accepted' if they have been taken from the notice board
    /// but a pilot is not currently assigned to them
    /// </summary>
    /// <returns></returns>
    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && GetScheduledMission(x) == null
            && !x.IsInProgress())
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
        // Send a thank you email on first completion of the Mission.
        if (scheduled.Mission.ThankYouMessage != null && scheduled.Mission.NumberOfCompletions <= 0)
        {
            scheduled.Mission.ThankYouMessage.IsUnlocked = true;
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

        // We will set the archived Mission fields throughout the outcome processing. 
        scheduled.Mission.ProcessOutcomes();

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(scheduled.Mission.MissionToArchive);

        ScheduledMissions.Remove(scheduled);
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
            if (scheduled?.Pilot != null && scheduled?.Pilot.Ship != null && scheduled?.Pilot.Ship == ship)
            {
                return scheduled;
            }
        }
        return null;
    }

    public static ScheduledMission GetScheduledMissionAtSlot(int node)
    {
        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if (scheduled?.Mission != null && scheduled?.Pilot != null && scheduled?.Pilot.Ship != null
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
            ScheduledMissions.Remove(GetScheduledMission(mission));
            Debug.Log("Scheduled Mission removed: " + GetScheduledMissionString(scheduledMission));
        }
    }

    public static void RemoveScheduledMission(Pilot pilot)
    {
        ScheduledMission scheduledMission = GetScheduledMission(pilot);
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(GetScheduledMission(pilot));
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
        if (scheduled != null)
        {
            return $"{scheduled.Mission}, {scheduled.Pilot}";
        }
        return string.Empty;
    }

    private static void LogScheduledMissions()
    {
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
        //string json = JsonHelper.ArrayToJson(ScheduledMissions.ToArray());
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
        //ScheduledMissions = JsonHelper.ArrayFromJson<ScheduledMission>(json).ToList();
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
