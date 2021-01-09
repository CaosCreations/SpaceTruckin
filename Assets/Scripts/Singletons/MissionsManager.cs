using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    public MissionContainer missionContainer;
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
            .Where(x => x.saveData.hasBeenAccepted
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
                    mission.saveData.ship.shipSaveData.isLaunched = false;
                    mission.saveData.ship.shipSaveData.currentMission = null;
                    mission.saveData.ship = null;
                }
            }
        }
    }

    public void SaveAllData()
    {
        foreach (Mission mission in Instance.missionContainer.missions)
        {
            mission.SaveData();
        }
    }

    public void SaveUpdatedData()
    {
        foreach (Mission mission in Instance.missionsWithUpdatedData)
        {
            mission.SaveData();
        }
    }

    public void RegisterUpdatedData(IDataModel dataModel)
    {
        Instance.missionsWithUpdatedData.Add(dataModel);
    }

    public void LoadData()
    {
        foreach (Mission mission in Instance.missionContainer.missions)
        {
            mission.LoadData();
        }
    }

    public void DeleteData(IDataModel dataModel)
    {
        dataModel.DeleteData();
    }

    public void DeleteAllData()
    {
        foreach (Mission mission in Instance.missionsWithUpdatedData)
        {
            mission.DeleteData();
        }
    }
}
