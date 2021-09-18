using System;

public static class TimeSpanExtensions
{
    public static TimeOfDay ToTimeOfDay(this TimeSpan self)
    {
        int seconds = Convert.ToInt32(self.TotalSeconds);
        TimeOfDay timeOfDay = CalendarUtils.ConvertSecondsToTimeOfDay(seconds);
        return timeOfDay;
    }
}
