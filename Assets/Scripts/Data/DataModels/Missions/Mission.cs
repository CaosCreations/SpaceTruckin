using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public enum MissionUnlockCondition
{
    TotalMoney, ConversationNode
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Missions/Mission", order = 1)]
public partial class Mission : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private string missionName, customer, cargo, description;
    [SerializeField] private int missionDurationInDays, fuelCost;
    [SerializeField] private MissionUnlockCondition unlockCondition;
    [SerializeField] private long moneyNeededToUnlock;
    [SerializeField] private bool isRepeatable = true; // Default to repeatable missions
    [SerializeField] private bool hasRandomOutcomes;
    [SerializeField] private MissionOutcome[] outcomes;
    [SerializeField] private MissionModifier missionModifier;

    [SerializeField] private float successChance;
    private bool wasSuccessful;

    [Tooltip("The Pilot traits that affect this mission, and whether the effect is positive/negative")]
    [SerializeField] private MissionPilotTraitEffects missionPilotTraitEffects;

    [Tooltip("The number of relationship points with the customer awarded on first completion of the mission")]
    [SerializeField] private int fondnessGranted;

    [Tooltip("The time after which there are consequences for not completing the mission")]
    [SerializeField] private int offerTimeLimitInDays;

    [Tooltip("The number of relationship points that are deducted if the time limit is exceeded")]
    [SerializeField] private int offerExpiryFondnessDeduction;

    [SerializeField] private ThankYouMessage thankYouMessage;

    [Header("Data to update IN GAME")]
    public MissionSaveData saveData;

    private ArchivedMission missionToArchive;

    public const string FolderName = "MissionSaveData";

    [Serializable]
    public class MissionSaveData
    {
        // Unlocked - appear in noticeboard ready to be accepted. 
        // Accepted - appear in office terminal and can be assigned to pilots. 
        public bool hasBeenUnlocked, hasBeenAccepted;

        // Track this so consequences of not actioning an offer aren't applied multiple times.
        public bool offerExpiryConsequencesApplied;

        public int daysLeftToComplete, numberOfCompletions;
        public Date dateUnlocked, dateAccepted;
        public MissionBonus missionBonus;
    }

    private void OnValidate()
    {
        missionDurationInDays = Mathf.Max(1, missionDurationInDays);
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MissionSaveData>(name, FolderName);
    }

    public void UnlockIfConditionMet()
    {
        switch (UnlockCondition)
        {
            case MissionUnlockCondition.TotalMoney:
                // This is called back by the Player Manager's OnFinancialTransaction() event. 
                if (CanBeUnlockedWithMoney)
                {
                    UnlockMission();

                    // This mission is unlocked, so it no longer needs to be notified of any money changes. 
                    PlayerManager.OnMoneySpent -= UnlockIfConditionMet;
                }
                break;

            case MissionUnlockCondition.ConversationNode:
                // This is called back by the Dialogue System's OnExecute() event.
                // The event fires when an associated conversation node is reached. 
                UnlockMission();
                break;
        }
    }

    private void UnlockMission()
    {
        if (HasBeenUnlocked)
        {
            // Prevent the Date from being reset if the mission is unlocked twice.
            return;
        }

        HasBeenUnlocked = true;
        DateUnlocked = CalendarManager.CurrentDate;
    }

    public void AcceptMission()
    {
        HasBeenAccepted = true;
        DateAccepted = CalendarManager.CurrentDate;
    }

    public void StartMission()
    {
        saveData.daysLeftToComplete = missionDurationInDays;
    }

    public bool IsInProgress()
    {
        return saveData.daysLeftToComplete > 0;
    }

    private IEnumerable<MissionOutcome> GetApplicableOutcomes(Mission mission)
    {
        return mission.Outcomes.Where(
            outcome => outcome != null && (mission.WasSuccessful || !outcome.RequiresMissionSuccess));
    }

    public void ProcessOutcomes()
    {
        // Some outcomes won't get processed based on mission success criteria.
        var applicableOutcomes = GetApplicableOutcomes(this);

        foreach (MissionOutcome outcome in applicableOutcomes)
        {
            if (outcome != null)
            {
                outcome.Process(MissionsManager.GetScheduledMission(this));
            }
        }
    }

    public override string ToString()
    {
        return $"Mission with name '{Name}'";
    }
}
