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
    [SerializeField] private long moneyNeededToUnlock;
    [SerializeField] private bool hasRandomOutcomes;
    [SerializeField] private MissionOutcome[] outcomes;
    [SerializeField] private ThankYouMessage thankYouMessage;

    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    [HideInInspector]
    private ArchivedMission missionToArchive;

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
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MissionSaveData>(name, FOLDER_NAME);
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

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            if (outcome != null)
            {
                outcome.Process(this);
            }
        }
    }

    private void OnValidate()
    {
        // Prevent the ship from being permanently launched 
        missionDurationInDays = Math.Max(1, missionDurationInDays);
    }
}
