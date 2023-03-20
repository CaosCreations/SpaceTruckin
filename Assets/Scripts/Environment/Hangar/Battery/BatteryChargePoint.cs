using System.Collections;
using UnityEngine;

public class BatteryChargePoint : InteractableObject
{
    [SerializeField]
    private float chargeTimeInSeconds = 2f;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(PlayerConstants.ActionKey) && IsPlayerInteractable)
        {
            BatteryCharging batteryCharging = other.GetComponentInChildren<BatteryCharging>();

            if (batteryCharging != null && !batteryCharging.IsCharged)
            {
                StartCoroutine(ChargeBattery(batteryCharging));
            }
        }
    }

    private IEnumerator ChargeBattery(BatteryCharging batteryCharging)
    {
        Debug.Log("Charge battery routine starting...");
        yield return new WaitForSeconds(chargeTimeInSeconds);

        Debug.Log("Charge battery routine finished.");
        batteryCharging.Charge();
    }

    protected override bool IsIconVisible => 
        IsPlayerInteractable 
        && HangarManager.CurrentBatteryBeingHeld != null 
        && !HangarManager.CurrentBatteryBeingHeld.BatteryCharging.IsCharged;

    private void OnValidate()
    {
        chargeTimeInSeconds = Mathf.Max(0f, chargeTimeInSeconds);
    }
}
