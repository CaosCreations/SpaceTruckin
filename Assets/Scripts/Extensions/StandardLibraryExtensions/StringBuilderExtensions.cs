using System.Text;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendLineWithBreaks(this StringBuilder self, string value, int extraBreaks = 0)
    {
        self.AppendLine(value.InsertNewLines());
        
        if (extraBreaks > 0)
        {
            for (int i = 0; i < extraBreaks; i++)
            {
                self.AppendLine();
            }
        }
        return self;
    }
}
