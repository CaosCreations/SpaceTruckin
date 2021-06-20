public static class TimeElementsExtensions
{
    public static int ToSeconds(this TimeOfDay self)
    {
        int seconds = (self.Hours * 60 + self.Minutes) * 60 + self.Seconds;
        return seconds; 
    }
}
