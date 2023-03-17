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
        bool canTransferEnergy = CanTransferEnergy(batteryCharging);

        if (canTransferEnergy)
        {
            // Shake hangar camera
            StationCameraManager.Instance.ShakeCamera(StationCamera.Identifier.Hangar);

            ShipsManager.EnableWarp(hangarSlot.Ship);
            batteryCharging.Discharge();
        }

        LogInformation(batteryCharging, canTransferEnergy);
    }

    private bool CanTransferEnergy(BatteryCharging battery)
    {
        return hangarSlot != null
            && hangarSlot.Ship != null
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();
            if (batteryCharging != null)
            {
                TransferEnergyToShip(batteryCharging);
            }
        }
    }

    #region Diagnostics
    private void LogInformation(BatteryCharging batteryCharging, bool chargeWasSuccessful)
    {
        StringBuilder builder = new StringBuilder();

        if (chargeWasSuccessful && hangarSlot.Ship.CanWarp)
        {
            builder.AppendLine($"{hangarSlot.Ship.Name} can now warp");
        }
        else
        {
            builder.AppendLine($"Failed to charge battery");

            if (hangarSlot == null)
            {
                builder.AppendLine($"HangarSlot is null");
            }
            else
            {
                if (hangarSlot.Ship == null)
                {
                    builder.AppendLine($"Ship is null");
                }
                else if (hangarSlot.Ship.CanWarp)
                {
                    builder.AppendLine($"{hangarSlot.Ship.Name} (Ship) can already warp");
                }
                if (!batteryCharging.IsCharged)
                {
                    builder.AppendLine($"{batteryCharging} (Battery) is not charged");
                }
            }
        }

        Debug.Log(builder.ToString());
    }
    #endregion
}
