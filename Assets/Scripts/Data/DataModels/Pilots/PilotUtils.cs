using System;

public class PilotUtils
{
    private static readonly Random random = new Random();

    public static T GetRandomEnumElement<T>() where T : Enum
    {
        Array possibleValues = Enum.GetValues(typeof(T));

        T randomElement = (T)possibleValues
            .GetValue(random.Next(0, possibleValues.Length));

        return randomElement;
    }

    public static Species GetRandomSpecies()
    {
        return GetRandomEnumElement<Species>();

        //Array possibleValues = Enum.GetValues(typeof(Species));

        //Species randomSpecies = (Species)possibleValues
        //    .GetValue(random.Next(0, possibleValues.Length));

        //return randomSpecies;
    }

    public static PilotAttributeType GetRandomAttributeType()
    {
        return GetRandomEnumElement<PilotAttributeType>();
    }
}
