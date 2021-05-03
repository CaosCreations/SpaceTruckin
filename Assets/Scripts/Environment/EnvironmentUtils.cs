using System.Linq;
using UnityEngine;

public static class EnvironmentUtils
{
    public static OfficeDoor GetDoorCollidingWithPlayer()
    {
        var collidingDoor = 
        /*return */Object
            .FindObjectsOfType<OfficeDoor>()
            .FirstOrDefault(d => d.IsPlayerColliding);

        return collidingDoor;
    }
}
