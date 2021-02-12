using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    public Mission[] Missions { get => missionContainer.missions; }

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
        if (DataModelsUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Mission.FOLDER_NAME);
        }
    }

    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Ship != null)
            .ToList();
    }

    public static Mission GetMissionByFileName(string fileName)
    {
        return Instance.Missions.FirstOrDefault(x => x.name == fileName);
    }

    public static void UpdateMissionSchedule()
    {
        // Reset yesterday's missions, so today's will take their place. 
        ArchivedMissionsManager.Instance.MissionsCompletedYesterday.Clear();

        foreach (Mission mission in Instance.Missions)
        {
            if(mission.IsInProgress())
            {
                mission.DaysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    mission.CompleteMission();
                }
            }
        }
    }

    private static void CompleteMission(Mission mission)
    {
        // Send a thank you email on first completion of the mission.
        if (mission.ThankYouMessage != null && mission.NumberOfCompletions <= 0)
        {
            mission.ThankYouMessage.IsUnlocked = true;
        }
        mission.NumberOfCompletions++;

        // Instantiate an archived mission object to store the stats of the completed mission.
        mission.MissionToArchive = new ArchivedMission(mission, mission.NumberOfCompletions);

        // We will set the archived mission fields throughout the outcome processing. 
        mission.ProcessOutcomes();

        mission.Ship.DeductFuel();
        mission.Ship.IsLaunched = false;
        mission.Ship.CurrentMission = null;
        mission.Ship = null;

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(mission.MissionToArchive);
    }

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
        DataModelsUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
}
