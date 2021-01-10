using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public partial class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public int missionDurationInDays;
    public string missionName, customer, cargo, description;
    public int fuelCost, reward, moneyNeededToUnlock; // may need to be longs later
    public MissionOutcome[] outcomes;

    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    public static string FOLDER_NAME = "MissionSaveData";

    public class MissionSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool hasBeenAccepted = false;
        [SerializeField] public int daysLeftToComplete;
        [SerializeField] public Ship ship = null;
    }

    public void SaveData()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(missionName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(missionName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<MissionSaveData>(fileName, FOLDER_NAME);
    }

    public void ScheduleMission(Ship ship)
    {
        saveData.ship = ship;
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = missionDurationInDays;
    }

    public bool IsInProgress()
    {
        return saveData.daysLeftToComplete > 0;
    }

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(this);
        }
    }
}
