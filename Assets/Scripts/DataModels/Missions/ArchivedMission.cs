using System.Threading.Tasks;
using UnityEngine;

public class ArchivedMission
{

    public string missionName;
    public long totalMoneyEarned;
    public int totalDamageTaken;
    public int totalFuelLost;
    public int totalPilotXpGained;
    public Ship shipUsed;

    public string MissionName { get => missionName; set => missionName = value; }
    public long TotalMoneyEarned { get => totalMoneyEarned; set => totalMoneyEarned = value; }
    public int TotalDamageTaken { get => totalDamageTaken; set => totalDamageTaken = value; }
    public int TotalPilotXpGained { get => totalPilotXpGained; set => totalPilotXpGained = value; }
    public int TotalFuelLost { get => totalFuelLost; set => totalFuelLost = value; }
    public Ship ShipUsed { get => shipUsed; set => shipUsed = value; }

    //public ArchivedMission(Mission mission)
    //{
    //    MissionName = mission.Name;
    //    TotalMoneyEarned = 
    //}

    //[HideInInspector]
    //ArchivedMissionSaveData saveData;


    //public class ArchivedMissionSaveData
    //{
    //    public string missionName;
    //    public long totalMoneyEarned;
    //    public int totalDamageTaken;
    //    public int totalFuelLost;
    //    public int totalPilotXpGained;
    //    public Ship shipUsed;
    //}

    //public string MissionName { get => saveData.missionName; set => saveData.missionName = value; }
    //public long TotalMoneyEarned { get => saveData.totalMoneyEarned; set => saveData.totalMoneyEarned = value; }
    //public int TotalDamageTaken { get => saveData.totalDamageTaken; set => saveData.totalDamageTaken = value; }
    //public int TotalPilotXpGained { get => saveData.totalPilotXpGained; set => saveData.totalPilotXpGained = value; }
    //public int TotalFuelLost { get => saveData.totalFuelLost; set => saveData.totalFuelLost = value; }
    //public Ship ShipUsed { get => saveData.shipUsed; set => saveData.shipUsed = value; }

}
