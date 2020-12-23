using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MissionsManager : MonoBehaviour
{
    public static MissionsManager Instance { get; private set; }

    public MissionContainer missionContainer;

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
    }

    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.missionSaveData.hasBeenAcceptedInNoticeBoard
            && x.missionSaveData.ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.missionSaveData.hasBeenAcceptedInNoticeBoard
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
                    mission.missionSaveData.ship.isLaunched = false;
                    mission.missionSaveData.ship.currentMission = null;
                    mission.missionSaveData.ship = null;
                }
            }
        }
    }
}
