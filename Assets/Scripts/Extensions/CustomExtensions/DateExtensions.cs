using System;

public static class DateExtensions
{
    public static int ToDays(this Date self)
    {
        return CalendarUtils.ConvertDateToDays(self);
    }

    public static Date Validate(this Date self)
    {
        // Cannot be below 1 
        self.Day = Math.Max(self.Day, 1);
        self.Month = Math.Max(self.Month, 1);
        self.Year = Math.Max(self.Year, 1);

        //// Cannot exceed max days in month/max months in year 
        //self.Day = Math.Min(self.Day, CalendarManager.Instance.DaysInMonth);
        //self.Month = Math.Min(self.Month, CalendarManager.Instance.MonthsInYear);

        return self;
    }
}
