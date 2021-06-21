using System;

public static class DateExtensions
{
    public static int ToDays(this Date self)
    {
        return CalendarUtils.ConvertDateToDays(self);
    }

    public static Date Validate(this Date self)
    {
        Date validDate = new Date
        {
            // Cannot be below 1 
            Day = Math.Max(self.Day, 1),
            Month = Math.Max(self.Day, 1)
        };

        // Cannot exceed max days in month/max months in year 
        validDate.Day = Math.Min(self.Day, CalendarManager.Instance.DaysInMonth);
        validDate.Month = Math.Min(self.Month, CalendarManager.Instance.MonthsInYear);

        return validDate;
    }
}
