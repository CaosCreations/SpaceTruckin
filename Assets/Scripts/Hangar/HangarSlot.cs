﻿using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    public int node;
    public Ship ship;
    public ShipInstance shipInstance;
    public bool IsUnlocked { get; set; }

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
        }
    }
}
