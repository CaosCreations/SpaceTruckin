﻿using System;
using System.Threading.Tasks;
using UnityEngine;

public enum MissionUnlockCondition
{
    TotalMoney, ConversationNode
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public partial class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private int missionDurationInDays;
    [SerializeField] private string missionName, customer, cargo, description;
    [SerializeField] private int fuelCost;
    [SerializeField] private MissionUnlockCondition unlockCondition;
    [SerializeField] private long moneyNeededToUnlock;
    [SerializeField] private bool hasRandomOutcomes;
    [SerializeField] private MissionOutcome[] outcomes;
    [SerializeField] private ThankYouMessage thankYouMessage;

    [Header("Data to update IN GAME")] 
    public MissionSaveData saveData;

    [HideInInspector]
    private ArchivedMission missionToArchive;

    public const string FOLDER_NAME = "MissionSaveData";

    [Serializable]
    public class MissionSaveData
    {
        public bool hasBeenUnlocked, hasBeenAccepted;
        public int daysLeftToComplete, numberOfCompletions;
    }

    private void OnValidate()
    {
        missionDurationInDays = Mathf.Max(1, missionDurationInDays);
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MissionSaveData>(name, FOLDER_NAME);
    }

    [HideInInspector]
    // Parameterless so it can subscribe to OnFinancialTransaction.
    // We can overload it if needed to handle other conditions.
    public void UnlockIfConditionMet()
    {
        HasBeenUnlocked = CanBeUnlockedWithMoney;
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = missionDurationInDays;
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MissionSaveData>(name, FOLDER_NAME);
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
                outcome.Process(MissionsManager.GetScheduledMission(this));
            }
        }
    }
}
