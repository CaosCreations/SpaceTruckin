using System;

public class PilotUtils
{
    public static Species GetRandomSpecies()
    {
        Array possibleValues = Enum.GetValues(typeof(Species));
        Species randomSpecies = (Species)possibleValues
            .GetValue(UnityEngine.Random.Range(0, possibleValues.Length));

        return randomSpecies;
    }
}
