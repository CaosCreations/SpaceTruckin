using UnityEngine;

public interface IMissionOutcome
{
    void Process(Mission mission);
}

public class MissionOutcome : ScriptableObject, IMissionOutcome
{
    [Range(0, 100)]
    public int probability;
    public string flavourText;

    public virtual void Process(Mission mission) { }
}