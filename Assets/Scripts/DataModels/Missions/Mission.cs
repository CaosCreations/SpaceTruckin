using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    public class MissionSaveData
    {
        // Data to persist
        [Header("Data to update IN GAME")]
        public bool hasBeenAcceptedInNoticeBoard = false;
        public Ship ship = null;
        public int daysLeftToComplete;
    }

    [Header("Set in Editor")]
    public int missionDurationInDays;
    public string missionName;
    public string customer;
    public string cargo;
    public int reward;
    public int moneyNeededToUnlock;
    [SerializeField]
    public MissionOutcome[] outcomes;

    // Data to persist
    [Header("Data to update IN GAME")]
    public MissionSaveData missionSaveData;

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

    public void Save()
    {
        Debug.Log($"Saving level {missionName}");
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
        Debug.Log($"Finished Saving level {missionName}");
    }

    public void Load()
    {
        Debug.Log($"loading level {missionName}");
        string folderPath = Path.Combine(Application.persistentDataPath, missionName);
        string filePath = Path.Combine(folderPath, $"{missionName}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                //missionSaveData = JsonUtility.FromJson<PersistentLevelDataModel>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading mission {missionName}");
    }
}
