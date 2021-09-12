using System;

public static class TimeOfDayExtensions
{
    public static int ToSeconds(this TimeOfDay self)
    {
        int seconds = (self.Hours * 3600) + (self.Minutes * 60) + self.Seconds;
        return seconds;
    }

    public static int ToRealTimeSeconds(this TimeOfDay self)
    {
        int inGameSeconds = self.ToSeconds();
        int realTimeSeconds = inGameSeconds.ToRealTimeSeconds();
        return realTimeSeconds;
    }

    public static TimeSpan ToTimeSpan(this TimeOfDay self)
    {
        return new TimeSpan(self.Hours, self.Minutes, self.Seconds);
    }

    public static TimeOfDay Validate(this TimeOfDay self)
    {
        // Cannot be below 0 
        self.Seconds = Math.Max(self.Seconds, 0);
        self.Minutes = Math.Max(self.Minutes, 0);
        self.Hours = Math.Max(self.Hours, 0);

        // Cannot exceed upper bounds 
        self.Seconds = Math.Min(self.Seconds, 59);
        self.Minutes = Math.Min(self.Minutes, 59);
        self.Hours = Math.Min(self.Hours, 23);

        return self;
    }
}
