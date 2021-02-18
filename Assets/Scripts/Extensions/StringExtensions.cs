using System;

public static class StringExtensions
{
    public static string InsertNewLines(this string self)
    {
        return self.Replace("<br>", Environment.NewLine);
    }
}