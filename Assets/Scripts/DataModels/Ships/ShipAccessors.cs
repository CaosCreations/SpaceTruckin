using System.Linq;
using UnityEngine;

public partial class Ship
{
    // This class is just for property accessors. 
    // The fields are all located in Ship.cs. 

    public string Name => shipName;
    public bool IsOwned
    {
        get => saveData.isOwned; set => saveData.isOwned = value;
    }
    public bool IsLaunched
    {
        get => saveData.isLaunched; set => saveData.isLaunched = value;
    }

    // Owned but not out on a mission nor docked
    public bool IsInQueue => IsOwned && !IsLaunched && !HangarManager.ShipIsDocked(this);
    public int CurrentFuel
    {
        get => saveData.currentFuel; set => saveData.currentFuel = value;
    }
    public int MaxFuel { get => maxFuel; set => maxFuel = value; }
    public float CurrentHullIntegrity
    {
        get => saveData.currenthullIntegrity; set => saveData.currenthullIntegrity = value;
    }
    //public int HangarNode // To udpate // 
    //{
    //    get => saveData.hangarNode; set => saveData.hangarNode = value;
    //}
    public GameObject ShipPrefab
    {
        get => shipPrefab; set => shipPrefab = value;
    }
    public Sprite Avatar { get => shipAvatar; set => shipAvatar = value; }
    public Pilot Pilot => PilotsManager.Instance.Pilots.FirstOrDefault(x => x.Ship == this);
    public Mission CurrentMission => Pilot.CurrentMission;
}
