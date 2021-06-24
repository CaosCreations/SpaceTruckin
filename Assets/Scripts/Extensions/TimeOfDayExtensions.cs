using System;

public static class TimeOfDayExtensions
{
    public static int ToSeconds(this TimeOfDay self)
    {
        int seconds = (self.Hours * 60 + self.Minutes) * 60 + self.Seconds;
        return seconds; 
    }

    public static int ToRealTimeSeconds(this TimeOfDay self)
    {
        int seconds = (self.Hours * 60 + self.Minutes) * 60 + self.Seconds;
        int realTimeSeconds = seconds.ToRealTimeSeconds();
        return realTimeSeconds;
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
