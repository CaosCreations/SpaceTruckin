using System;

public static class StringExtensions
{
    public static string InsertNewLines(this string self)
    {
        if (!string.IsNullOrWhiteSpace(self))
        {
            return self.Replace("<br>", Environment.NewLine);
        }
        return string.Empty;
    }

    public static string ToItalics(this string self)
    {
        if (!string.IsNullOrWhiteSpace(self))
        {
            return self.Insert(self.Length - 1, "</i>").Insert(0, "<i>");
        }
        return string.Empty;
    }

    public static string RemoveAllWhitespace(this string self)
    {
        if (!string.IsNullOrEmpty(self))
        {
            return self.Replace(" ", string.Empty);
        }
        return string.Empty;
    }

    public static string RemoveCharacter(this string self, char character)
    {
        if (self != null && !char.IsWhiteSpace(character)) 
        {
            return self.Replace(character.ToString(), string.Empty);
        }
        return string.Empty;
    }
}
