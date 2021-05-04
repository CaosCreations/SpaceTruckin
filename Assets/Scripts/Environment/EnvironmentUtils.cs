using System.Linq;
using UnityEngine;

public static class EnvironmentUtils
{
    public static OfficeDoor GetDoorCollidingWithPlayer()
    {
        return Object
            .FindObjectsOfType<OfficeDoor>()
            .FirstOrDefault(d => d.IsPlayerColliding);
    }
}
