using System.Text;
using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;

    protected override bool IsIconVisible => HangarManager.CurrentBatteryBeingHeld != null 
        && IsPlayerInteractable 
        && CanTransferEnergy(HangarManager.CurrentBatteryBeingHeld.BatteryCharging);

    public void TransferEnergyToShip(BatteryCharging batteryCharging)
    {
        // Shake hangar camera
        StationCameraManager.Instance.ShakeCamera(StationCamera.Identifier.Hangar);

        ShipsManager.EnableWarp(hangarSlot.Ship);
        batteryCharging.Discharge();
    }

    private bool CanTransferEnergy(BatteryCharging battery)
    {
        return hangarSlot != null
            && hangarSlot.IsOccupied
            && battery != null
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp;
    }

    public bool CanTransferEnergy()
    {
        return HangarManager.CurrentBatteryBeingHeld != null 
            && CanTransferEnergy(HangarManager.CurrentBatteryBeingHeld.BatteryCharging);
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();
            if (CanTransferEnergy(batteryCharging))
            {
                TransferEnergyToShip(batteryCharging);
            }
        }
    }
}
