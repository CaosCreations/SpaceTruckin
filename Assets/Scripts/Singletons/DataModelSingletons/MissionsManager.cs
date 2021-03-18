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
    public static List<ScheduledMission> ScheduledMissions = new List<ScheduledMission>();

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
        }

        if (Missions == null)
        {
            Debug.LogError("No mission data found");
        }
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

    /// <summary>
    /// Missions are 'in progress' if the pilot assigned to them has left the hangar
    /// </summary>
    public static List<Mission> GetMissionsInProgress()
    {
        return Instance.Missions
            .Where(x => x.IsInProgress())
            .ToList();
    }

    /// <summary>
    /// Missions are 'in hangar' if they've been scheduled but their pilots haven't left the hangar
    /// </summary>
    /// <returns></returns>
    public static List<ScheduledMission> GetScheduledMissionsStillInHangar()
    {
        //return Instance.Missions
        //    .Where(x => x.HasBeenAccepted
        //    && GetScheduledMissionByMission(x) != null
        //    && !x.IsInProgress())
        //    .ToList();

        return ScheduledMissions.Where(x => !x.mission.IsInProgress()).ToList();
    }

    public static Mission GetMissionByFileName(string fileName)
    {
        return Instance.Missions.FirstOrDefault(x => x.name == fileName);
    }

    public static void UpdateMissionSchedule()
    {
        // Reset yesterday's missions, so today's will take their place. 
        ArchivedMissionsManager.ResetMissionsCompletedYesterday();

        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if(scheduled.mission.IsInProgress())
            {
                scheduled.mission.DaysLeftToComplete--;

                // We just finished the scheduledMission.mission
                if (!scheduled.mission.IsInProgress())
                {
                    CompleteMission(scheduled);
                }
            }
        }
    }

    private static void CompleteMission(ScheduledMission scheduled)
    {
        // Send a thank you email on first completion of the mission.
        if (scheduled.mission.ThankYouMessage != null && scheduled.mission.NumberOfCompletions <= 0)
        {
            scheduled.mission.ThankYouMessage.IsUnlocked = true;
        }
        scheduled.mission.NumberOfCompletions++;

        // Instantiate an archived mission object to store the stats of the completed mission.
        scheduled.mission.MissionToArchive = new ArchivedMission(scheduled.mission, scheduled.pilot, scheduled.mission.NumberOfCompletions);

        scheduled.pilot.MissionsCompleted++;
        scheduled.mission.MissionToArchive.MissionsCompletedByPilotAtTimeOfMission = scheduled.pilot.MissionsCompleted;

        // Randomise the mission's outcomes if flag is set or they are missing. 
        if (scheduled.mission.HasRandomOutcomes)
        {
            AssignRandomOutcomes(scheduled.mission);
        }

        // We will set the archived mission fields throughout the outcome processing. 
        scheduled.mission.ProcessOutcomes();

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(scheduled.mission.MissionToArchive);

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
        return ScheduledMissions.FirstOrDefault(x => x.mission == mission);
    }

    public static ScheduledMission GetScheduledMission(Pilot pilot)
    {
        return ScheduledMissions.FirstOrDefault(x => x.pilot == pilot);
    }

    public static ScheduledMission GetScheduledMission(Ship ship)
    {
        return ScheduledMissions.FirstOrDefault(x => x.pilot.Ship == ship);
    }

    public static void AddOrUpdateScheduledMission(Pilot pilot, Mission mission)
    {
        ScheduledMission scheduledMission = GetScheduledMission(mission);
        if (scheduledMission != null)
        {
            scheduledMission.pilot = pilot;
        }
        else
        {
            scheduledMission = new ScheduledMission { pilot = pilot, mission = mission };
            ScheduledMissions.Add(scheduledMission);
        }
        Debug.Log("Scheduled Mission added/updated:\n" + GetScheduledMissionString(scheduledMission));
    }

    public static void RemoveScheduledMission(ScheduledMission scheduledMission)
    {
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(scheduledMission);
            Debug.Log("Scheduled Mission removed:\n" + GetScheduledMissionString(scheduledMission));
        }
    }

    public static void RemoveScheduledMission(Mission mission)
    {
        ScheduledMission scheduledMission = GetScheduledMission(mission);
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(GetScheduledMission(mission));
            Debug.Log("Scheduled Mission removed:\n" + GetScheduledMissionString(scheduledMission));
        }
    }

    public static void RemoveScheduledMission(Pilot pilot)
    {
        ScheduledMission scheduledMission = GetScheduledMission(pilot);
        if (scheduledMission != null)
        {
            ScheduledMissions.Remove(GetScheduledMission(pilot));
            Debug.Log("Scheduled Mission removed:\n" + GetScheduledMissionString(scheduledMission));
        }
    }

    private static string GetScheduledMissionString(ScheduledMission scheduled)
    {
        if (scheduled != null)
        {
            return $"Mission: {scheduled.mission}, Pilot: {scheduled.pilot}";
        }
        return string.Empty;
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            mission.SaveData();
        }
    }

    public async void LoadDataAsync()
    {
        foreach (Mission mission in Instance.Missions)
        {
            await mission.LoadDataAsync();
        }
    }
    
    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
    #endregion
}
