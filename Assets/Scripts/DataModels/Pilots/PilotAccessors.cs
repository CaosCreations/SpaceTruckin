using System.Linq;
using UnityEngine;

public partial class Pilot
{
    // This class is just for property accessors. 
    // The fields are all located in Pilot.cs. 

    public string Name { get => pilotName; }

    public string Description
    {
        get => description; set => description = value;
    }

    public int HireCost { get => hireCost; }

    public int Xp { get => saveData.xp; set => saveData.xp = value; }

    public int Level { get => saveData.level; set => saveData.level = value; }

    public int MissionsCompleted
    {
        get => saveData.missionsCompleted; set { saveData.missionsCompleted = value; }
    }

    public bool IsHired
    {
        get => saveData.isHired; set { saveData.isHired = value; }
    }

    public bool IsOnMission
    {
        get => saveData.isOnMission; set { saveData.isOnMission = value; }
    }

    public bool IsAssignedToShip
    {
        get => saveData.isAssignedToShip; set { saveData.isAssignedToShip = value; }
    }

    public Ship Ship { get => ShipsManager.Instance.Ships.FirstOrDefault(s => s.Pilot == this); } 

    public Sprite Avatar { get => avatar; set => avatar = value; }
}
