using System.Linq;
using UnityEngine;

public partial class Ship
{
    // This class is just for property accessors. 
    // The fields are all located in Ship.cs. 

    public string Name => shipName;
    public bool IsLaunched => ShipsManager.ShipIsLaunched(this);

    // Owned but not out on a mission nor docked
    public bool IsInQueue => !IsLaunched && !HangarManager.ShipIsDocked(this);
    
    public int CurrentFuel
    {
        get => saveData.currentFuel; set => saveData.currentFuel = value;
    }
    public int MaxFuel { get => maxFuel; set => maxFuel = value; }
    public float CurrentHullIntegrity
    {
        get => saveData.currenthullIntegrity; set => saveData.currenthullIntegrity = value;
    }
    public GameObject ShipPrefab
    {
        get => shipPrefab; set => shipPrefab = value;
    }
    public Sprite Avatar { get => shipAvatar; set => shipAvatar = value; }
    public Pilot Pilot => PilotsManager.Instance.Pilots.FirstOrDefault(x => x.Ship == this);
    public Mission CurrentMission => MissionsManager.GetMission(this);
}
