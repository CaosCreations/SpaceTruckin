using UnityEngine;

public static class CalendarUtils
{
    public static int ConvertDateToDays(Date date)
    {
        // Subtract 1 as years and months start at 1, not 0. 
        int yearsInDays = (date.Year - 1) * CalendarManager.MonthsInYear * CalendarManager.DaysInMonth;
        int monthsInDays = (date.Month - 1) * CalendarManager.DaysInMonth;

        return yearsInDays + monthsInDays + date.Day;
    }

    // Used for compatibility with Lua functions
    public static double ConvertDateToDays(double day, double month, double year)
    {
        // Subtract 1 as years and months start at 1, not 0. 
        double yearsInDays = (year - 1) * CalendarManager.MonthsInYear * CalendarManager.DaysInMonth;
        double monthsInDays = (month - 1) * CalendarManager.DaysInMonth;

        return yearsInDays + monthsInDays + day;
    }

    public static Date ConvertDaysToDate(int days)
    {
        int years = Mathf.FloorToInt(days / CalendarManager.DaysInYear);
        days %= CalendarManager.DaysInYear;

        int months = Mathf.FloorToInt(days / CalendarManager.DaysInMonth);
        days %= CalendarManager.DaysInMonth;

        return new Date() { Day = days, Month = months, Year = years };
    }

    public static TimeOfDay ConvertSecondsToTimeOfDay(int seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 3600);
        seconds %= 3600;

        int minutes = Mathf.FloorToInt(seconds / 60);
        seconds %= 60;

        return new TimeOfDay() { Hours = hours, Minutes = minutes, Seconds = seconds };
    }

    public static bool HasTimePeriodElapsed(Date startingDate, Date period)
    {
        return ConvertDateToDays(CalendarManager.CurrentDate) - ConvertDateToDays(startingDate)
            > ConvertDateToDays(period);
    }

    public static bool HasTimePeriodElapsed(Date startingDate, int periodInDays)
    {
        return ConvertDateToDays(CalendarManager.CurrentDate) - ConvertDateToDays(startingDate)
            > periodInDays;
    }
}
