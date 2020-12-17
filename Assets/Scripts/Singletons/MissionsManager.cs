using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsManager : MonoBehaviour
{
    public static MissionsManager Instance { get; private set; }

    public MissionContainer missionContainer;
    public List<Mission> missionsAcceptedInNoticeBoard;

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
        missionsAcceptedInNoticeBoard = new List<Mission>();

        foreach(Mission mission in Instance.missionContainer.missions)
        {
            if (mission.hasBeenAcceptedInNoticeBoard)
            {
                missionsAcceptedInNoticeBoard.Add(mission);
            }
        }
    }
}
