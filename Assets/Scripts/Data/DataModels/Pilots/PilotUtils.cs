using System;

public class PilotUtils
{
    private static readonly Random random = new Random();
    public static Species GetRandomSpecies()
    {
        Array possibleValues = Enum.GetValues(typeof(Species));

        Species randomSpecies = (Species)possibleValues
            .GetValue(random.Next(0, possibleValues.Length));

        return randomSpecies;
    }
}
