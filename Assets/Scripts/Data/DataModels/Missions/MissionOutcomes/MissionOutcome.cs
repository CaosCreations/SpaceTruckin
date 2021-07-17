using UnityEngine;

public interface IMissionOutcome
{
    void Process(ScheduledMission mission);
}

public class MissionOutcome : ScriptableObject, IMissionOutcome
{
    [Range(0, 100)]
    public int Probability;

    [SerializeField] private string flavourText;

    public virtual void Process(ScheduledMission mission) { }
}
