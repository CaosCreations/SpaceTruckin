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

    public TimeOfDay(int hours, int minutes, int seconds)
    {
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }

    public enum Phase
    {
        Morning, Evening
    }
}
