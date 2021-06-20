public static class DateExtensions
{
    public static int ToDays(this Date self)
    {
        return CalendarUtils.ConvertDateToDays(self);
    }
}
