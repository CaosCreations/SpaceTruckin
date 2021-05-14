using System.Collections.Generic;
using UnityEngine;

public static class HashSetExtensions
{
    public static List<KeyCode> ToListOfKeyCodes(this HashSet<KeyCodeOverride> self)
    {
        var listOfKeyCodes = new List<KeyCode>();

        foreach (var keyCodeOverride in self)
        {
            if (keyCodeOverride != null)
            {
                listOfKeyCodes.Add(keyCodeOverride.KeyCode);
            }
        }
        return listOfKeyCodes;
    }

    public static List<KeyCode> ToListOfNonPersistentKeyCodes(this HashSet<KeyCodeOverride> self)
    {
        var listOfKeyCodes = new List<KeyCode>();

        foreach (var keyCodeOverride in self)
        {
            if (keyCodeOverride != null && !keyCodeOverride.IsPersistentOnDisable)
            {
                listOfKeyCodes.Add(keyCodeOverride.KeyCode);
            }
        }
        return listOfKeyCodes;
    }
}
