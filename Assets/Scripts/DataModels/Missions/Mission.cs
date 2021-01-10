using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")] 
    public MissionData data;
    
    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    public static string FOLDER_NAME = "MissionSaveData";

    // Non-persistent data
    public bool HasBeenAccepted
    {
        get { return saveData.hasBeenAccepted; }
        set { saveData.hasBeenAccepted = value; }
    }

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
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = data.missionDurationInDays;
    }
    
    public bool IsInProgress()
    {
        return saveData.daysLeftToComplete > 0;
    }

    public void SaveData()
    {
        string fileName = $"{data.missionName}_{saveData.guid}";
        DataModelsUtils.SaveFile(fileName, FOLDER_NAME, saveData);
    }

    public async void LoadData()
    {
        string fileName = $"{data.missionName}_{saveData.guid}";
        saveData = await DataModelsUtils.LoadFile<MissionSaveData>(fileName, FOLDER_NAME);
    }
}
