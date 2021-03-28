using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;

    public void TransferEnergyToShip(Battery battery)
    {
        if (hangarSlot != null
            && hangarSlot.Ship != null
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp)
        {
            ShipsManager.EnableWarp(hangarSlot.Ship);
            battery.Discharge();
            Debug.Log($"{hangarSlot.Ship.Name} (Ship) can now warp - launch condition fulfilled");
        }
        else
        {
            Debug.Log("Could not transfer energy");
        }
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
}
