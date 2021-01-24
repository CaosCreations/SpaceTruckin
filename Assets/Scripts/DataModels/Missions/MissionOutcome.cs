using System;
using UnityEngine;

public interface IMissionOutcome
{
    void Process(Mission mission);
}

public class MissionOutcome : ScriptableObject, IMissionOutcome, IDataModel
{
    public int probability;
    public string flavourText;

    public MissionOutcomeSaveData saveData;

    [Serializable]
    public class MissionOutcomeSaveData
    {
        // Data common to all types of outcomes. 
        // e.g. Date completed. 
    }

    public virtual void Process(Mission mission) { }
}