using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            Battery battery = other.GetComponentInChildren<Battery>();
            
            if (battery != null && !battery.IsCharged)
            {
                battery.Charge();
            }
            Debug.Log("Charger action");
        }
    }
}