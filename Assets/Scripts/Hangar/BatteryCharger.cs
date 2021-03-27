using UnityEngine;

public class BatteryCharger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(HangarConstants.BatteryTag)
            && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            Battery battery = other.GetComponent<Battery>();
            
            if (battery != null
                && !battery.IsCharged
                && battery.IsPlayerHolding())
            {
                battery.Charge();
            }
            Debug.Log("Charger action");
        }
    }
}