using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    public HangarNode node;
    public ShipInstance shipInstance;

    public void LaunchShip()
    {
        shipInstance.Launch();
    }
}
