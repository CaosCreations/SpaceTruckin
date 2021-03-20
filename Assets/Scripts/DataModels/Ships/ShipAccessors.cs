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
        //get => PilotsManager.PilotHasMission(this, missionIsInProgress: true);
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
    public GameObject ShipPrefab
    {
        get => shipPrefab; set => shipPrefab = value;
    }
    public Sprite Avatar { get => shipAvatar; set => shipAvatar = value; }
    public Pilot Pilot => PilotsManager.Instance.Pilots.FirstOrDefault(x => x.Ship == this);
    public Mission CurrentMission
    {
        get
        {
            ScheduledMission scheduled = MissionsManager.GetScheduledMission(this);
            if (scheduled != null && scheduled.Mission != null)
            {
                return scheduled.Mission;
            }
            return null;
        }
    }
}
