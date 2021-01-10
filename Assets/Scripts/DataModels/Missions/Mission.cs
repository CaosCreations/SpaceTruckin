using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")] 
    public MissionData data;
    
    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    private const string FOLDER_NAME = "MissionSaveData";

    // Non-persistent data
    public class MissionData
    {
        public int missionDurationInDays;
        public string missionName, customer, cargo, description;
        public int fuelCost, reward, moneyNeededToUnlock; // may need to be longs later
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

    public string MissionName 
    {
        get => data.missionName; set => data.missionName = value; 
    }

    public string Customer
    {
        get => data.customer; set => data.customer = value;
    }
    
    public string Cargo { get => data.cargo; set => data.cargo = value; }

    public int Reward { get => data.reward; set => data.reward = value; }

    public bool HasBeenAccepted
    {
        get => saveData.hasBeenAccepted; 
        set => saveData.hasBeenAccepted = value;
    }

    public int DaysLeftToComplete
    {
        get => saveData.daysLeftToComplete; 
        set => saveData.daysLeftToComplete = value;
    }

    public int FuelCost { get => data.fuelCost; set => data.fuelCost = value; }

    public int MoneyNeededToUnlock
    {
        get => data.moneyNeededToUnlock; set => data.moneyNeededToUnlock = value;
    }

    public Ship Ship { get => saveData.ship; set => saveData.ship = null; }

    public MissionOutcome[] Outcomes { get => data.outcomes; }

    public void SaveData()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(data.missionName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(data.missionName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<MissionSaveData>(fileName, FOLDER_NAME);
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

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in data.outcomes)
        {
            outcome.Process(this);
        }
    }
}
