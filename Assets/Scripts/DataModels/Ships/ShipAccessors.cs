using System.Linq;
using UnityEngine;

public partial class Ship
{
    // This class is just for property accessors. 
    // The fields are all located in Ship.cs. 

    public string Name { get => shipName; }

    public bool IsOwned
    {
        get => saveData.isOwned; set => saveData.isOwned = value;
    }

    public bool IsLaunched
    {
        get => saveData.isLaunched; set => saveData.isLaunched = value;
    }

    public int CurrentFuel
    {
        get => saveData.currentFuel; set => saveData.currentFuel = value;
    }

    public int MaxFuel { get => maxFuel; set => maxFuel = value; }

    public float CurrentHullIntegrity
    {
        get => saveData.currenthullIntegrity; set => saveData.currenthullIntegrity = value;
    }

    public HangarNode HangarNode
    {
        get => saveData.hangarNode; set => saveData.hangarNode = value;
    }

    //public Mission CurrentMission
    //{
    //    get => saveData.currentMission; set => saveData.currentMission = value;
    //}

    public Mission CurrentMission
    {
        get => MissionsManager.Instance.Missions
            .FirstOrDefault(m => m.saveData.id == saveData.currentMissionId);
    }

    public GameObject ShipPrefab
    {
        get => shipPrefab; set => shipPrefab = value;
    }

    public Sprite Avatar { get => shipAvatar; set => shipAvatar = value; }

    public Pilot Pilot { get => pilot; set => pilot = value; }
}
