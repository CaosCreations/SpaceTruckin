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
            .Where(x => x.hasBeenAcceptedInNoticeBoard
            && x.ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.missionContainer.missions
            .Where(x => x.hasBeenAcceptedInNoticeBoard
            && x.ship != null)
            .ToList();
    }

    public static void UpdateMissionSchedule()
    {
        foreach(Mission mission in Instance.missionContainer.missions)
        {
            if(mission.IsInProgress()){
                mission.daysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    mission.ship.isLaunched = false;
                    mission.ship.currentMission = null;
                    mission.ship = null;
                }
            }
        }
    }
}
