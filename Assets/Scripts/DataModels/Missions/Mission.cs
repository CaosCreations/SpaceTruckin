using UnityEngine;

public enum MissionSource
{
    Noticeboard = 1, Email = 2, Npc = 3
}

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    [Header("Set in Editor")]
    public int missionDurationInDays;
    public string missionName;
    public string customer;
    public string cargo;
    public string description;
    public int fuelCost;
    public int reward;
    public int moneyNeededToUnlock;
    [SerializeField]
    public MissionOutcome[] outcomes;

    // Data to persist
    [Header("Data to update IN GAME")]
    public bool hasBeenAccepted = false;
    public Ship ship = null;
    public int daysLeftToComplete;


    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(this);
        }
    }

    public void ScheduleMission(Ship ship)
    {
        this.ship = ship;
    }

    public void StartMission()
    {
        daysLeftToComplete = missionDurationInDays;
    }
    
    public bool IsInProgress()
    {
        return daysLeftToComplete > 0;
    }
}
