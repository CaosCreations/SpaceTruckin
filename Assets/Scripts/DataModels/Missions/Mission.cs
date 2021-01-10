using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")] 
    public MissionData data;
    
    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData; 

    // Non-persistent data
    public class MissionData
    {
        public int missionDurationInDays;
        public string missionName, customer, cargo, description;
        public int fuelCost, reward, moneyNeededToUnlock;
        public MissionOutcome[] outcomes;
    }

    // Persistent data 
    public class MissionSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool hasBeenAccepted = false;
        [SerializeField] public int daysLeftToComplete;
        [SerializeField] public Ship ship = null;
    }

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in data.outcomes)
        {
            outcome.Process(this);
        }
    }

    public void ScheduleMission(Ship ship)
    {
        saveData.ship = ship;
        //ShipsManager.RegisterUpdatedMission
        DataModelsUtils.SaveData(this);
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = data.missionDurationInDays;
        MissionsManager.Instance.RegisterUpdatedData(this);
    }
    
    public bool IsInProgress()
    {
        return saveData.daysLeftToComplete > 0;
    }

    public void SaveData()
    {
        Debug.Log($"Saving mission: {data.missionName}_{saveData.guid}");
        
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
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

        string filePath = Path.Combine(folderPath, $"{data.missionName}_{saveData.guid}.save");
        string fileContents = JsonUtility.ToJson(saveData);

        string fileContents = JsonUtility.ToJson(
            new MissionSaveData() 
            { 
                guid = saveData.guid, hasBeenAccepted = saveData.hasBeenAccepted, daysLeftToComplete = saveData.daysLeftToComplete, ship = saveData.ship 
            
            });
        try
        {
            File.WriteAllText(filePath, fileContents);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        Debug.Log($"Finished saving mission: {data.missionName}_{saveData.guid}");

    }

    public void LoadData()
    {
        Debug.Log($"loading level: {data.missionName}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"{data.missionName}_{saveData.guid}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<MissionSaveData>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading level: {data.missionName}_{saveData.guid}");
    }

    public void DeleteData()
    {
        Debug.Log($"Deleting mission: {data.missionName}_{saveData.guid}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"/{data.missionName}_{saveData.guid}.save");
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
