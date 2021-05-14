using System.Collections.Generic;
using UnityEngine;

public static class HashSetExtensions
{
    /// <summary>
    /// Extract the keycodes from a set of keycode override objects into a list.
    /// </summary>
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

    /// <summary>
    /// Extract the keycodes from a set of keycode override objects into a list.
    /// The elements of the set must have their persistence flag set to false. 
    /// </summary>
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
