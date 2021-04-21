using UnityEngine;

public class BatterySlot : InteractableObject
{
    [SerializeField] private HangarSlot hangarSlot;
    [SerializeField] private GameObject batteryReceiver;
    public Battery BatteryInSlot { get; set; }
    private bool SlotIsEmpty => BatteryInSlot == null;

    public void InsertBattery(Battery batteryToInsert)
    {
        if (batteryToInsert != null)
        {
            BatteryInSlot = batteryToInsert;
            BatteryInSlot.Container.SetParent(gameObject);

            // Snap the battery into the slot
            BatteryInSlot.transform.position = batteryReceiver.transform.position;
            BatteryInSlot.transform.rotation = batteryReceiver.transform.rotation;
        }
    }

    public void TransferEnergyToShip(Battery battery)
    {
        if (hangarSlot != null
            && hangarSlot.Ship != null
            && battery.IsCharged
            && !hangarSlot.Ship.CanWarp)
        {
            ShipsManager.EnableWarp(hangarSlot.Ship);
            battery.Discharge();
            Debug.Log($"{hangarSlot.Ship.Name} (Ship) can now warp");
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
            if (battery == null)
            {
                return;
            }

            if (SlotIsEmpty)
            {
                InsertBattery(battery);
            }
            else if (battery.IsCharged)
            {
                TransferEnergyToShip(battery);
            }

            // Todo: handle removing battery from slot 
        }
    }
}
