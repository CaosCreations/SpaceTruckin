using System;

public class PilotUtils
{
    public static Species GetRandomSpecies()
    {
        return GetRandomEnumElement<Species>();
    }

    public static PilotAttributeType GetRandomAttributeType()
    {
        return GetRandomEnumElement<PilotAttributeType>();
    }

    private static T GetRandomEnumElement<T>() where T : Enum
    {
        T[] possibleValues = (T[])Enum.GetValues(typeof(T));

        T randomElement = possibleValues.GetRandomElement();

        return randomElement;
    }
}
