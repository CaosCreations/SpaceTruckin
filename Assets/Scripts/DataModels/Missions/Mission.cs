using System;
using System.IO;
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

    [Serializable]
    public class MissionSaveData
    {
        [SerializeField] public int id, shipId; 
        [SerializeField] public bool hasBeenAccepted = false;
        [SerializeField] public int daysLeftToComplete;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        //saveData = await DataModelsUtils.LoadFileAsync<MissionSaveData>(name, FOLDER_NAME);
        string path = Path.Combine(Application.persistentDataPath, FOLDER_NAME, name + ".truckin");
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, FOLDER_NAME, name + ".truckin"));
        saveData = JsonUtility.FromJson<MissionSaveData>(json);
    }   

    public void ScheduleMission(Ship ship/*, int id*/)
    {
        Ship = ship;
        //saveData.shipId = id;
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
