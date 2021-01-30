using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
        //Init();
    }

    public void Init()
    {
        Init();


        if (DataModelsUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Mission.FOLDER_NAME);
        }
        //Mission.OnMissionCompleted += (mission) => 
        //    MissionsCompletedYesterday.Add((mission, mission.Ship));
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
