using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;

    private void Start()
    {
    }

    public void TransferEnergyToShip(Battery battery)
    {
        if (hangarSlot != null && hangarSlot.Ship != null)
        {
            ShipsManager.EnableWarp(hangarSlot.Ship);
            battery.Discharge();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            Battery battery = other.GetComponentInChildren<Battery>();
            if (battery != null && battery.IsCharged)
            {
                TransferEnergyToShip(battery);
            }
        }
    }
}
