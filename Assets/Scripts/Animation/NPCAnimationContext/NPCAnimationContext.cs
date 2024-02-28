using System;
using UnityEngine;

[Serializable]
public class NPCAnimationContext
{
    [field: SerializeField]
    public string MorningParameterName { get; private set; }

    [field: SerializeField]
    public string EveningParameterName { get; private set; }

    [field: SerializeField]
    public string MorningStateName { get; private set; }

    [field: SerializeField]
    public string EveningStateName { get; private set; }

    public string GetParameterNameByPhase(TimeOfDay.Phase phase)
    {
        return phase == TimeOfDay.Phase.Morning ? MorningParameterName : EveningParameterName;
    }

    public string GetStateNameByPhase(TimeOfDay.Phase phase)
    {
        return phase == TimeOfDay.Phase.Morning ? MorningStateName : EveningStateName;
    }
}
