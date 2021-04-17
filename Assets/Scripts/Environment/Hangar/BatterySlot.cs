using System.Text;
using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;

    public void TransferEnergyToShip(Battery battery)
    {
        bool canTransferEnergy = CanTransferEnergy(battery);

        if (canTransferEnergy)
        {
            ShipsManager.EnableWarp(hangarSlot.Ship);
            battery.Discharge();
        }
        
        LogInformation(battery, canTransferEnergy);
    }

    private bool CanTransferEnergy(Battery battery)
    {
        return hangarSlot != null
            && hangarSlot.Ship != null
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKey(PlayerConstants.ActionKey))
        {
            Battery battery = other.GetComponentInChildren<Battery>();
            if (battery != null && battery.IsCharged)
            {
                TransferEnergyToShip(battery);
            }
        }
    }

    #region Diagnostics
    private void LogInformation(Battery battery, bool chargeWasSuccessful)
    {
        StringBuilder builder = new StringBuilder();

        if (chargeWasSuccessful && hangarSlot.Ship.CanWarp)
        {
            builder.AppendLine($"{hangarSlot.Ship.Name} can now warp");
        }
        else
        {
            builder.AppendLine($"Failed to charge {hangarSlot.Ship.Name} at node {hangarSlot.Node}");

            if (hangarSlot == null)
            {
                builder.AppendLine($"{hangarSlot} (HangarSlot) is null");
            }
            else
            {
                if (hangarSlot.Ship == null)
                {
                    builder.AppendLine($"{hangarSlot.Ship} (Ship) is null");
                }
                else if (hangarSlot.Ship.CanWarp)
                {
                    builder.AppendLine($"{hangarSlot.Ship} (Ship) can already warp");
                }
                if (!battery.IsCharged)
                {
                    builder.AppendLine($"{battery} (Battery) is not charged");
                }
            }
        }

        Debug.Log(builder.ToString());
    }
    #endregion
}
