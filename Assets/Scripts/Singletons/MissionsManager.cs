using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    public MissionContainer missionContainer;
    public Mission[] Missions { get => missionContainer.missions; }
    public List<IDataModel> missionsWithUpdatedData;

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
        return Instance.missionContainer.missions
            .Where(x => x.HasBeenAccepted
            && x.saveData.ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.saveData.hasBeenAccepted
            && x.saveData.ship != null)
            .ToList();
    }

    public static void UpdateMissionSchedule()
    {
        foreach(Mission mission in Instance.missionContainer.missions)
        {
            if(mission.IsInProgress()){
                mission.saveData.daysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    mission.Ship.IsLaunched = false;
                    mission.Ship.CurrentMission = null;
                    mission.saveData.ship = null;
                }
            }
        }
    }

    public void SaveData()
    {
        foreach (Mission mission in Instance.missionContainer.missions)
        {
            mission.SaveData();
        }
    }

    public async void LoadData()
    {
        foreach (Mission mission in Instance.missionContainer.missions)
        {
            await mission.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        Instance.DeleteData();
    }
}
