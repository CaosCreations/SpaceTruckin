using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;

    protected override bool IsIconVisible => HangarManager.CurrentBatteryBeingHeld != null
        && IsPlayerInteractable
        && CanTransferEnergy(HangarManager.CurrentBatteryBeingHeld.BatteryCharging);

    public void TransferEnergyToShip(BatteryCharging batteryCharging)
    {
        bool canTransferEnergy = CanTransferEnergy(batteryCharging);

        if (canTransferEnergy)
        {
            // Shake hangar camera
            StationCameraManager.ShakeCamera(StationCamera.Identifier.Hangar);

            ShipsManager.EnableWarp(hangarSlot.Ship);
            batteryCharging.Discharge();
        }
    }

    private bool CanTransferEnergy(BatteryCharging battery)
    {
        return hangarSlot != null
            && hangarSlot.IsOccupied
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerInteractable
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();
            if (batteryCharging != null)
            {
                TransferEnergyToShip(batteryCharging);
            }
        }
    }
}
