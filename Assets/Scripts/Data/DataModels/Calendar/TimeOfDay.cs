using System;

/// <summary>
/// Represents a time of day in a format that can be set in the inspector.
/// </summary>
[Serializable]
public struct TimeOfDay
{
    public int Hours;
    public int Minutes;
    public int Seconds;

    public enum TimeOfDayPhase
    {
        Morning, Evening
    }
}
