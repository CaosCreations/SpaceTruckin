using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    private void OnTriggerStay(Collider other)
    {
        if (IsPlayerColliding
            && Input.GetKey(PlayerConstants.ActionKey)
            && PlayerManager.IsFirstRaycastHit(HangarConstants.BatteryChargerLayer, gameObject))
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();

            if (batteryCharging != null && !batteryCharging.IsCharged)
            {
                batteryCharging.Charge();
            }
        }
    }
}
