using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public partial class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private int missionDurationInDays;
    [SerializeField] private string missionName, customer, cargo, description;
    [SerializeField] private int fuelCost;
    [SerializeField] private long reward, moneyNeededToUnlock;
    [SerializeField] private MissionOutcome[] outcomes;
    [SerializeField] private ThankYouMessage thankYouMessage;

    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    [HideInInspector]
    private ArchivedMission archivedMission; 

    public static event Action<Mission> OnMissionCompleted;
    public static string FOLDER_NAME = "MissionSaveData";

    [Serializable]
    public class MissionSaveData
    {
        public bool hasBeenAccepted = false;
        public int daysLeftToComplete, numberOfCompletions;
        public Ship ship = null;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<MissionSaveData>(name, FOLDER_NAME);
    }   

    public void ScheduleMission(Ship ship)
    {
        Ship = ship;
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = missionDurationInDays;
    }

    public bool IsInProgress()
    {
        return saveData.daysLeftToComplete > 0;
    }

    private void ProcessOutcomes()
    {
        LatestArchivedMission = new ArchivedMission();

        foreach (MissionOutcome outcome in outcomes)
        {
            if (outcome != null)
            {
                outcome.Process(this);
            }
        }
    }

    public void CompleteMission()
    {
        ProcessOutcomes();
        Ship.DeductFuel();
        Ship.IsLaunched = false;
        Ship.CurrentMission = null;
        Ship = null;

        // Send a thank you email on first completion of the mission
        if (thankYouMessage != null && NumberOfCompletions <= 0)
        {
            thankYouMessage.IsUnlocked = true; 
        }
        NumberOfCompletions++;
        OnMissionCompleted?.Invoke(this);
    }
}
