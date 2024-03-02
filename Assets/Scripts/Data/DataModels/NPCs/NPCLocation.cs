using System;
using UnityEngine;

[Serializable]
public class NPCLocation
{
    [field: SerializeField]
    public Vector3 MorningStationPosition { get; private set; }

    [field: SerializeField]
    public Vector3 EveningStationPosition { get; private set; }

    [field: SerializeField]
    public Condition[] Conditions { get; private set; }

    [field: SerializeField]
    public ConditionGroup MorningConditions { get; private set; }

    [field: SerializeField]
    public ConditionGroup EveningConditions { get; private set; }

    public Vector3 GetPositionByPhase(TimeOfDay.Phase phase)
    {
        return phase == TimeOfDay.Phase.Morning ? MorningStationPosition : EveningStationPosition;
    }
}