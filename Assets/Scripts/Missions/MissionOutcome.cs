using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMissionOutcome
{
    void Process(Mission mission);
}

public class MissionOutcome : ScriptableObject, IMissionOutcome
{
    public int probability;

    public virtual void Process(Mission mission)
    {
    }
}