using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission", order = 1)]
public class Mission : ScriptableObject
{
    public string pilotsName;
    public int missionDurationInSeconds;
    public string missionName;

    [SerializeField]
    public MissionOutcome[] outcomes;

    public bool inProgress;

    public void ProcessOutcomes()
    {
        foreach (MissionOutcome outcome in outcomes)
        {
            outcome.Process();
        }
    }
}
