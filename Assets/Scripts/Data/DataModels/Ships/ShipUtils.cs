using System;

public static class ShipUtils
{
    public static ShipDamageType GetRandomDamageType()
    {
        return GetRandomEnumElement<ShipDamageType>();
    }

    private static T GetRandomEnumElement<T>() where T : Enum
    {
        T[] possibleValues = (T[])Enum.GetValues(typeof(T));

        T randomElement = possibleValues.GetRandomElement();

        return randomElement;
    }
}
