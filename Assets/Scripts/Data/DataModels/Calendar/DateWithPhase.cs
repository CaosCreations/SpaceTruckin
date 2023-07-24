using UnityEngine;

public class DateWithPhase
{
    [field: SerializeField]
    public Date Date { get; private set; }

    [field: SerializeField]
    public TimeOfDay.Phase Phase { get; private set; }
}
