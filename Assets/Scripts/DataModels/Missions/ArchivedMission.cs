using UnityEngine;

public class ArchivedMission
{
    private string missionName;
    private long totalMoneyEarned;
    private int totalDamageTaken;
    private int totalFuelLost;
    private int totalPilotXpGained; 
    private Ship shipUsed;

    public string MissionName { get => missionName; set => missionName = value; }
    public long TotalMoneyEarned { get => totalMoneyEarned; set => totalMoneyEarned = value; }
    public int TotalDamageTaken { get => totalDamageTaken; set => totalDamageTaken = value; }
    public int TotalPilotXpGained { get => totalPilotXpGained; set => totalPilotXpGained = value; }
    public int TotalFuelLost { get => totalFuelLost; set => totalFuelLost = value; }
    public Ship ShipUsed { get => shipUsed; set => shipUsed = value; }
}
