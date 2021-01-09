using System;
using System.IO;
using UnityEngine;

public enum MissionSource
{
    Noticeboard = 1, Email = 2, Npc = 3
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    public class MissionSaveData
    {
        // Data to persist
        public bool hasBeenAccepted = false;
        public Ship ship = null;
        public int daysLeftToComplete;
    }

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
    }

    public void StartMission()
    {
        missionSaveData.daysLeftToComplete = missionDurationInDays;
    }
    
    public bool IsInProgress()
    {
        return missionSaveData.daysLeftToComplete > 0;
    }

    public void SaveMissionData()
    {
        // Append a guid or use existing mission id to handle duplicates?
        Debug.Log($"Saving mission: {missionName}");
        string folderPath = Path.Combine(Application.persistentDataPath, missionName);
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
}
