using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MissionsManager : MonoBehaviour
{
    public static MissionsManager Instance { get; private set; }

    public MissionContainer missionContainer;
    public List<Mission> missionsWithUpdatedData; 

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
        // Load data here
    }

    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.missionSaveData.hasBeenAccepted
            && x.missionSaveData.ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.missionSaveData.hasBeenAccepted
            && x.missionSaveData.ship != null)
            .ToList();
    }

    public static void UpdateMissionSchedule()
    {
        foreach(Mission mission in Instance.missionContainer.missions)
        {
            if(mission.IsInProgress()){
                mission.missionSaveData.daysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    mission.missionSaveData.ship.shipSaveData.isLaunched = false;
                    mission.missionSaveData.ship.shipSaveData.currentMission = null;
                    mission.missionSaveData.ship = null;
                }
            }
        }
    }

    public static void SavePersistentDataForUpdatedMissions()
    {
        foreach (Mission mission in Instance.missionsWithUpdatedData)
        {
            mission.SaveData();
        }
    }

    public static void RegisterUpdatedMission(Mission mission)
    {
        Instance.missionsWithUpdatedData.Add(mission);
    }

    public static void SavePersistentDataForAllMissions()
    {
        foreach (Mission mission in Instance.missionContainer.missions)
        {
            mission.SaveData();
        }
    }
}
