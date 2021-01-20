public class PlaceholderUtils
{
    public static string LoremIpsumPassage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tortor dui, elementum eu convallis non, cursus ac dolor. Quisque dictum est quam, et pellentesque velit rutrum eget. Nullam interdum ultricies velit pharetra aliquet. Integer sodales a magna quis ornare. Ut vulputate nibh ipsum. Vivamus tincidunt nec nisi in fermentum. Mauris consequat mi vel odio consequat, eget gravida urna lobortis. Pellentesque eu ipsum consectetur, pharetra nulla in, consectetur turpis. Curabitur ornare eu nisi tempus varius. Phasellus vel ex mauris. Fusce fermentum mi id elementum gravida. ";

    public static string GenerateLoremIpsum(int passageCount = 1)
    {
        string loremIpsum = string.Empty;
        for (int i = 0; i < passageCount; i++)
        {
            loremIpsum += LoremIpsumPassage;
        }
        return loremIpsum;
    }
}
