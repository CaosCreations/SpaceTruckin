using System;
using System.Linq;
using UnityEngine;

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

    public static T GetRandomEnumElement<T>() where T : Enum
    {
        T[] possibleValues = (T[])Enum.GetValues(typeof(T));

        T randomElement = possibleValues.GetRandomElement();

        return randomElement;
    }

    public static Pilot GetPilotByName(string name)
    {
        var pilot = PilotsManager.Instance.Pilots.FirstOrDefault(pilot => pilot.Name == name);
        if (pilot == null)
        {
            Debug.LogError("Pilot doesn't exist with name: " + name);
        }
        return pilot;
    }
}
