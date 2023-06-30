using Events;
using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;
    public int Node => hangarSlot.Node;

    [SerializeField] private BatterySlotHoldingLocation holdingLocation;

    protected override bool IsIconVisible => HangarManager.CurrentBatteryBeingHeld != null
        && IsPlayerInteractable
        && CanTransferEnergy(HangarManager.CurrentBatteryBeingHeld.BatteryCharging);

    public void TransferEnergyToShip(BatteryCharging batteryCharging)
    {
        StationCameraManager.Instance.ShakeCamera(StationCamera.Identifier.Hangar);
        holdingLocation.PutInSlot();
        ShipsManager.EnableWarp(hangarSlot.Ship);
        batteryCharging.Discharge();
        SingletonManager.EventService.Dispatch(new OnShipChargedEvent(Node));
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
