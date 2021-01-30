﻿using System;
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
        // Send a thank you email on first completion of the mission.
        if (ThankYouMessage != null && NumberOfCompletions <= 0)
        {
            ThankYouMessage.IsUnlocked = true;
        }
        NumberOfCompletions++;

        // Instantiate an archived mission object to store the stats of the completed mission.
        MissionToArchive = new ArchivedMission(this, NumberOfCompletions);

        // We will set the archived mission fields throughout the outcome processing. 
        ProcessOutcomes();
        
        Ship.DeductFuel();
        Ship.IsLaunched = false;
        Ship.CurrentMission = null;
        Ship = null;

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(MissionToArchive);
    }
}
