using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    [Header("Set in Editor")]
    public int missionDurationInDays;
    public string missionName;
    public string customer;
    public string cargo;
    public int reward;
    public int moneyNeededToUnlock;

    // Data to persist
    [Header("Data to update IN GAME")]
    public bool hasBeenAcceptedInNoticeBoard = false;
    public HangarNode scheduledHangarNode = HangarNode.None;
    public Pilot currentPilot = null;


    [SerializeField]
    public MissionOutcome[] outcomes;

    public bool inProgress;

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process(this);
        }
    }
}
