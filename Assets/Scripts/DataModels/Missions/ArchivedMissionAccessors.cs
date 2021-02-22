public partial class ArchivedMission
{
    // This class is just for property accessors. 
    // The fields are all located in ArchivedMission.cs. 

    public string MissionName { get => saveData.missionName; set => saveData.missionName = value; }
    public int CompletionNumber { get => saveData.completionNumber; set => saveData.completionNumber = value; }
    public long TotalMoneyEarned { get => saveData.totalMoneyEarned; set => saveData.totalMoneyEarned = value; }
    public int TotalDamageTaken { get => saveData.totalDamageTaken; set => saveData.totalDamageTaken = value; }
    public int PilotLevelAtTimeOfMission
    {
        get => saveData.pilotLevelAtTimeOfMission; set => saveData.pilotLevelAtTimeOfMission = value;
    }
    public int MissionsCompletedByPilotAtTimeOfMission
    {
        get => saveData.missionsCompletedByPilotAtTimeOfMission;
        set => saveData.missionsCompletedByPilotAtTimeOfMission = value;
    }
    public double TotalPilotXpGained { get => saveData.totalPilotXpGained; set => saveData.totalPilotXpGained = value; }
    public int TotalFuelLost { get => saveData.totalFuelLost; set => saveData.totalFuelLost = value; }
    public Ship Ship { get => saveData.ship; set => saveData.ship = value; }
    public Pilot Pilot => Ship.Pilot;
}
