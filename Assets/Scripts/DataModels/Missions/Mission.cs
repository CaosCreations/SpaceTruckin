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

    public Ship Ship { get => saveData.ship; set => saveData.ship = null; }

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
