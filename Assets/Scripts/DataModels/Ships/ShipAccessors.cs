using System.Linq;
using UnityEngine;

public partial class Ship
{
    // This class is just for property accessors. 
    // The fields are all located in Ship.cs. 

    public string Name => shipName;
    public bool IsLaunched => ShipsManager.ShipIsLaunched(this);

    /// <summary>Owned but not out on a mission nor docked</summary>
    public bool IsInQueue => Pilot.IsHired && !IsLaunched && !HangarManager.ShipIsDocked(this);
    
    public int CurrentFuel
    {
        get => saveData.currentFuel; set => saveData.currentFuel = value;
    }
    public int MaxFuel { get => maxFuel; set => maxFuel = value; }
    public float CurrentHullIntegrity
    {
        get => saveData.currentHullIntegrity; set => saveData.currentHullIntegrity = value;
    }
    
    /// <summary>Warp is required to go out on missions</summary>
    public bool CanWarp { get => saveData.canWarp; set => saveData.canWarp = value; }
    
    public GameObject ShipPrefab
    {
        get => shipPrefab; set => shipPrefab = value;
    }
    public Sprite Avatar { get => shipAvatar; set => shipAvatar = value; }
    public Pilot Pilot => PilotsManager.Instance.Pilots.FirstOrDefault(x => x.Ship == this);
    public Mission CurrentMission => MissionsManager.GetMission(this);
}
