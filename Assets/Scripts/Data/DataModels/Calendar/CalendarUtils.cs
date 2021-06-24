using UnityEngine;

public static class CalendarUtils
{
    public static int ConvertDateToDays(Date date)
    {
        // Subtract 1 as years and months start at 1, not 0. 
        int yearsInDays = (date.Year - 1) * CalendarManager.Instance.MonthsInYear * CalendarManager.Instance.DaysInMonth;
        int monthsInDays = (date.Month - 1) * CalendarManager.Instance.DaysInMonth;

        return yearsInDays + monthsInDays + date.Day;
    }

    // Used for compatibility with Lua functions
    public static double ConvertDateToDays(double day, double month, double year)
    {
        // Subtract 1 as years and months start at 1, not 0. 
        double yearsInDays = (year - 1) * CalendarManager.Instance.MonthsInYear * CalendarManager.Instance.DaysInMonth;
        double monthsInDays = (month - 1) * CalendarManager.Instance.DaysInMonth;

        return yearsInDays + monthsInDays + day;
    }

    public static Date ConvertDaysToDate(int days)
    {
        int years = Mathf.FloorToInt(days / CalendarManager.Instance.DaysInYear);
        days %= CalendarManager.Instance.DaysInYear;

        int months = Mathf.FloorToInt(days / CalendarManager.Instance.DaysInMonth);
        days %= CalendarManager.Instance.DaysInMonth;

        return new Date() { Day = days, Month = months, Year = years };
    }

    public static TimeOfDay ConvertSecondsToTimeOfDay(int seconds)
    {
        int hours = Mathf.FloorToInt(seconds / 60);
        seconds %= 60;

        int minutes = Mathf.FloorToInt(seconds / 60);
        seconds %= 60;

        return new TimeOfDay() { Hours = hours, Minutes = minutes, Seconds = seconds };
    }

    public static bool HasTimePeriodElapsed(Date startingDate, Date period)
    {
        return ConvertDateToDays(CalendarManager.Instance.CurrentDate) - ConvertDateToDays(startingDate)
            > ConvertDateToDays(period);
    }

    public static bool HasTimePeriodElapsed(Date startingDate, int periodInDays)
    {
        return ConvertDateToDays(CalendarManager.Instance.CurrentDate) - ConvertDateToDays(startingDate)
            > periodInDays;
    }
}
