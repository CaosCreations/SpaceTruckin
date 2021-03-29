﻿using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    [SerializeField] private int node;
    [SerializeField] private BatterySlot batterySlot;

    private Ship ship;
    private ShipInstance shipInstance;

    public void LaunchShip()
    {
        if (shipInstance != null)
        {
            shipInstance.Launch();
            Ship = null;
            ShipInstance = null;
        }
    }

    public int Node => node;
    public BatterySlot BatterySlot { get => batterySlot; set => batterySlot = value; }
    public Ship Ship { get => ship; set => ship = value; }
    public ShipInstance ShipInstance { get => shipInstance; set => shipInstance = value; }
    public bool IsUnlocked 
    { 
        get => Node <= LicencesManager.HangarSlotUnlockEffect + HangarConstants.StartingNumberOfSlots; 
    } 
    public bool IsOccupied { get => Ship != null; }
}