using System;

public static class StringExtensions
{
    public static string InsertNewLines(this string self)
    {
        return self.Replace("<br>", Environment.NewLine);
    }

    public static string ToItalics(this string self)
    {
        return self.Insert(0, "<i>").Insert(self.Length - 1, "</i>");
    }
}