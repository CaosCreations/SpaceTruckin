using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(PlayerConstants.ActionKey) && IsPlayerInteractable)
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();

            if (batteryCharging != null && !batteryCharging.IsCharged)
            {
                batteryCharging.Charge();
            }
        }
    }

    protected override bool IsIconVisible => 
        IsPlayerInteractable 
        && HangarManager.CurrentBatteryBeingHeld != null 
        && !HangarManager.CurrentBatteryBeingHeld.BatteryCharging.IsCharged;
}
