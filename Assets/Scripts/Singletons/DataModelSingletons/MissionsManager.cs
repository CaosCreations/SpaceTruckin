using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    public Mission[] Missions { get => missionContainer.missions; }

    /// <summary>
    /// Stores the missions that were completed the day before
    /// as well as money they yielded, which is determined by a dice roll
    /// </summary>
    public Dictionary<Mission, long> RecentlyCompletedMissions { get; private set; }

    void Awake()
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
        Init();
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
        MoneyOutcome.OnMoneyOutcome += AddRecentlyCompletedMission;
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

    public static void UpdateMissionSchedule()
    {
        foreach(Mission mission in Instance.Missions)
        {
            if(mission.IsInProgress()){
                mission.DaysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    mission.CompleteMission();
                }
            }
        }
    }

    private void AddRecentlyCompletedMission(Mission mission, long moneyReceived)
    {
        Instance.RecentlyCompletedMissions.Add(mission, moneyReceived);
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
