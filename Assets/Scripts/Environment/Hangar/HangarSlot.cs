using UnityEngine;

public class HangarSlot : MonoBehaviour
{
    [SerializeField] private int node;
    [SerializeField] private BatterySlot batterySlot;
    public Battery BatteryInSlot { get => batterySlot.BatteryInSlot; }
    public Ship Ship { get; set; }
    public ShipInstance ShipInstance { get; set; }

    public void LaunchShip()
    {
        if (ShipInstance != null)
        {
            ShipInstance.Launch();
            Ship = null;
            ShipInstance = null;
        }
    }

    public int Node => node;
    public bool IsUnlocked 
    { 
        get => Node <= LicencesManager.HangarSlotUnlockEffect + HangarConstants.StartingNumberOfSlots; 
    } 
    public bool IsOccupied { get => Ship != null; }
}
