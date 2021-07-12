using System.Linq;

public partial class Mission
{
    // This class is just for property accessors. 
    // The fields are all located in Mission.cs. 

    public string Name { get => missionName; set => missionName = value; }
    public string Customer { get => customer; set => customer = value; }
    public string Cargo { get => cargo; set => cargo = value; }
    public string Description { get => description; set => description = value; }
    public MissionUnlockCondition UnlockCondition { get => unlockCondition; }
    public bool CanBeUnlockedWithMoney
    {
        get => UnlockCondition == MissionUnlockCondition.TotalMoney
           && MoneyNeededToUnlock <= PlayerManager.Instance.Money;
    }
    public bool HasBeenUnlocked
    {
        get => saveData.hasBeenUnlocked;
        set => saveData.hasBeenUnlocked = value;
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
    public Date DateUnlocked
    {
        get => saveData.dateUnlocked; set => saveData.dateUnlocked = value;
    }
    public Date DateAccepted
    {
        get => saveData.dateAccepted; set => saveData.dateAccepted = value;
    }
    public int NumberOfCompletions
    {
        get => saveData.numberOfCompletions;
        set => saveData.numberOfCompletions = value;
    }
    public int FuelCost { get => fuelCost; set => fuelCost = value; }
    public long MoneyNeededToUnlock
    {
        get => moneyNeededToUnlock; set => moneyNeededToUnlock = value;
    }
    public int FondnessGranted => fondnessGranted;
    public int OfferTimeLimitInDays => offerTimeLimitInDays;
    public bool HasOfferExpired 
    {
        get => CalendarUtils.HasTimePeriodElapsed(DateAccepted, offerTimeLimitInDays); 
    }
    public bool OfferExpiryConsequencesApplied 
    {
        get => saveData.offerExpiryConsequencesApplied; 
        set => saveData.offerExpiryConsequencesApplied = value;
    }
    public int OfferExpiryFondnessDeduction => offerExpiryFondnessDeduction;
    public bool HasRandomOutcomes 
    { 
        get => hasRandomOutcomes || Outcomes == null || Outcomes.Length <= 0;
        set => hasRandomOutcomes = value; 
    }
    public bool IsRepeatable => isRepeatable;
    public bool IsAvailableForScheduling
    {
        get => HasBeenAccepted 
            && !IsScheduled
            && !IsInProgress()

            // If non-repeatable mission offers expire, they can no longer be scheduled. 
            && (IsRepeatable || (!OfferExpiryConsequencesApplied && NumberOfCompletions <= 0));
    }
    public bool IsScheduled => MissionsManager.GetScheduledMission(this) != null; 
    public MissionOutcome[] Outcomes { get => outcomes; set => outcomes = value; }
    public ArchivedMission MissionToArchive 
    {
        get => missionToArchive; set => missionToArchive = value; 
    }
    public ThankYouMessage ThankYouMessage 
    { 
        get => thankYouMessage; set => thankYouMessage = value; 
    }
    public bool HasModifier => missionModifier != null 
        && missionModifier.PossibleOutcomes != null
        && missionModifier.PossibleOutcomes.Any();

    public MissionModifier Modifier => missionModifier;
}
