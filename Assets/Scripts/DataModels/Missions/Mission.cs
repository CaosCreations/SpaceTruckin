using System;
using System.IO;
using UnityEngine;

public enum MissionSource
{
    Noticeboard = 1, Email = 2, Npc = 3
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject, IPersistentData
{
    public class MissionSaveData
    {
        public bool hasBeenAccepted = false;
        public int daysLeftToComplete;
        public Ship ship = null;
    }

    private string folderName = "Missions";

    [Header("Set in Editor")]
    public int missionDurationInDays;
    public string missionName, customer, cargo, description;
    public int fuelCost, reward, moneyNeededToUnlock;

    [Header("Data to update IN GAME")]
    public MissionSaveData missionSaveData; 

    [SerializeField]
    public MissionOutcome[] outcomes;

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(this);
        }
    }

    public void ScheduleMission(Ship ship)
    {
        missionSaveData.ship = ship;
        //ShipsManager.RegisterUpdatedMission
    }

    public void StartMission()
    {
        missionSaveData.daysLeftToComplete = missionDurationInDays;
        MissionsManager.RegisterUpdatedMission(this);
    }
    
    public bool IsInProgress()
    {
        return missionSaveData.daysLeftToComplete > 0;
    }

    public void SaveData()
    {
        // Mission name must be unique 
        Debug.Log($"Saving mission: {missionName}");
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if (!Directory.Exists(folderPath))
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        string filePath = Path.Combine(folderPath, $"{missionName}.save");
        string fileContents = JsonUtility.ToJson(missionSaveData);
        try
        {
            File.WriteAllText(filePath, fileContents);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        Debug.Log($"Finished saving mission: {missionName}");

    }

    public void LoadData()
    {
        Debug.Log($"loading level {missionName}");
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string filePath = Path.Combine(folderPath, $"{missionName}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                missionSaveData = JsonUtility.FromJson<MissionSaveData>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading level {missionName}");
    }

    public void Clear()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, missionName);
        string filePath = Path.Combine(folderPath, $"/{missionName}.save");
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
