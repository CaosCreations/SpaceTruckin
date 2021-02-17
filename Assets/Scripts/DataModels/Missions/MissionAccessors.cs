public partial class Mission
{
    // This class is just for property accessors. 
    // The fields are all located in Mission.cs. 

    public string Name { get => missionName; set => missionName = value; }

    public string Customer { get => customer; set => customer = value; }

    public string Cargo { get => cargo; set => cargo = value; }

    public string Description { get => description; set => description = value; }

    public long Reward { get => reward; set => reward = value; }

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

    public Ship Ship { get => saveData.ship; set => saveData.ship = value; }
    public Pilot Pilot { get => saveData.ship.Pilot; }

    public MissionOutcome[] Outcomes { get; set; }

    public ArchivedMission MissionToArchive 
    {
        get => missionToArchive; set => missionToArchive = value; 
    }

    public ThankYouMessage ThankYouMessage 
    { 
        get => thankYouMessage; set => thankYouMessage = value; 
    }
}
