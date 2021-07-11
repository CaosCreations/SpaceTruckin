﻿using System;

public class PilotUtils
{
    private static readonly Random random = new Random();

    public static T GetRandomEnumElement<T>() where T : Enum
    {
        T[] possibleValues = (T[])Enum.GetValues(typeof(T));

        T randomElement = (T)possibleValues.GetRandomElement();

        return randomElement;
    }

    public static Species GetRandomSpecies()
    {
        return GetRandomEnumElement<Species>();
    }

    public static PilotAttributeType GetRandomAttributeType()
    {
        return GetRandomEnumElement<PilotAttributeType>();
    }
}
