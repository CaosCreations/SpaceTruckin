using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    public Mission[] Missions { get => missionContainer.missions; }

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

    void Init()
    {
        LoadData();
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
                    mission.Ship.IsLaunched = false;
                    mission.Ship.CurrentMission = null;
                    mission.Ship = null;
                }
            }
        }
    }

    public void SaveData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            mission.SaveData();
        }
    }

    public async void LoadData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            await mission.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        Instance.DeleteData();
    }
}
