using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMissionOutcome
{
    void Process();
}

public class MissionOutcome : ScriptableObject, IMissionOutcome
{
    public virtual void Process()
    {
        throw new System.NotImplementedException();
    }
}