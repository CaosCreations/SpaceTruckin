using System.Text;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLineWithBreaks(this StringBuilder self, string value)
    {
        return self.AppendLine(value.InsertNewLines());
    }
}
