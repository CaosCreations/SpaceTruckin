using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    [SerializeField] private MissionOutcomeContainer missionOutcomeContainer;
    public Mission[] Missions { get => missionContainer.missions; }
    public MissionOutcome[] Outcomes { get => missionOutcomeContainer.missionOutcomes; }
    public static List<ScheduledMission> ScheduledMissions;
    private object folderPath;

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

        if (Missions == null || Missions.Length <= 0)
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

    /// <summary>
    /// Missions are 'in progress' if the pilot assigned to them has left the hangar
    /// </summary>
    public static List<Mission> GetMissionsInProgress()
    {
        return Instance.Missions
            .Where(x => x.IsInProgress())
            .ToList();
    }

    public static List<ScheduledMission> GetScheduledMissionsNotInProgress()
    {
        //return Instance.Missions
        //    .Where(x => x.HasBeenAccepted
        //    && GetScheduledMissionByMission(x) != null
        //    && !x.IsInProgress())
        //    .ToList();

        return ScheduledMissions.Where(x => x.Mission != null && !x.Mission.IsInProgress()).ToList();
    }

    public static Mission GetMissionByFileName(string fileName)
    {
        return Instance.Missions.FirstOrDefault(x => x.name == fileName);
    }

    public static void UpdateMissionSchedule()
    {
        // Reset yesterday's Missions, so today's will take their place. 
        ArchivedMissionsManager.ResetMissionsCompletedYesterday();

        foreach (ScheduledMission scheduled in ScheduledMissions.ToList())
        {
            if (scheduled.Mission == null || scheduled.Pilot == null)
            {
                // Outcome processing depends on Missions and Pilots 
                RemoveScheduledMission(scheduled);
                continue;
            }

            if(scheduled.Mission.IsInProgress())
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

    public static Mission GetMissionByObjectName(string objectName)
    {
        return Instance.Missions.FirstOrDefault(x => x.name == objectName);
    }

    public static ScheduledMission GetScheduledMission(Mission mission)
    {
        return ScheduledMissions.FirstOrDefault(x => x.Mission == mission);
    }

    public static ScheduledMission GetScheduledMission(Pilot pilot)
    {
        return ScheduledMissions.FirstOrDefault(x => x.Pilot == pilot);
    }

    public static ScheduledMission GetScheduledMission(Ship ship)
    {
        foreach (ScheduledMission scheduled in ScheduledMissions)
        {
            if (scheduled.Pilot != null && scheduled.Pilot.Ship != null && scheduled.Pilot.Ship == ship)
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
            return $"Mission: {scheduled.Mission}, Pilot: {scheduled.Pilot}";
        }
        return string.Empty;
    }

    private static void LogScheduledMissions()
    {
        ScheduledMissions.ForEach(x => Debug.Log(GetScheduledMissionString(x)));
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            mission.SaveData();
        }
        //foreach (ScheduledMission scheduled in ScheduledMissions)
        //{
        //    scheduled.SaveData();
        //}
        // Write scheduled missions to a single file 
        //ScheduledMission[] scheduledMissionsToSave = ScheduledMissions.ToArray();
        //string json = JsonUtility.ToJson(scheduledMissionsToSave);
        //DataUtils.SaveFileAsync("ScheduledMissionSaveData", Mission.FOLDER_NAME, json);

        var sm = ScheduledMissions.ToArray();
        string json = JsonHelper.ArrayToJson(sm);
        Debug.Log("Log SM saving Json: " + json);
        string folderPath = DataUtils.GetSaveFolderPath(Mission.FOLDER_NAME);
        DataUtils.SaveFileAsync("ScheduledMissionSaveData", Mission.FOLDER_NAME, json);


    }

    public async void LoadDataAsync()
    {
        foreach (Mission mission in Instance.Missions)
        {
            await mission.LoadDataAsync();
        }

        string folderPath = DataUtils.GetSaveFolderPath(Mission.FOLDER_NAME);
        string filePath = Path.Combine(folderPath, "ScheduledMissionSaveData.truckin");
        string json = await DataUtils.ReadTextFileAsync(filePath);
        Debug.Log("Log SM loading Json: " + json);
        ScheduledMissions = new List<ScheduledMission>(); // simplify
        ScheduledMissions = JsonHelper.ArrayFromJson<ScheduledMission>(json).ToList();
        Debug.Log("Logging SM Json post-load:\n");
        LogScheduledMissions();


        //string[] scheduledMissionFiles = Directory.GetFiles(ScheduledMission.FOLDER_NAME);
        //foreach (string file in scheduledMissionFiles)
        //{
        //    string[] objectNameComponents = file.Split('_');
        //    //Mission mission = GetMissionByObjectName(objectNameComponents[0]);
        //    //Pilot pilot = PilotsManager.GetPilotByObjectName(objectNameComponents[1]);


        //    //ScheduledMission scheduledMission = new ScheduledMission(mission, pilot);


        //}

        //foreach ()
    }
    
    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
    #endregion
}
