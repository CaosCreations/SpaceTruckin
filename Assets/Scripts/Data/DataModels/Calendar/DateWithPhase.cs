using System;
using UnityEngine;

[Serializable]
public class DateWithPhase
{
    [field: SerializeField]
    public Date Date { get; private set; }

    [field: SerializeField]
    public TimeOfDay.Phase Phase { get; private set; }
}
