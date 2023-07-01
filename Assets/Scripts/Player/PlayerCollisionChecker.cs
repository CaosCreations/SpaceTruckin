using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
    private void Update()
    {
        // TODO: Could use OverlapBoxNonAlloc later if we know a maximum collider count 
        var colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f);

        var isBatterySlotColliding = false;
        var isChargerColliding = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(HangarConstants.BatterySlotTag) && collider.gameObject.TryGetComponent<BatterySlot>(out var batterySlot))
            {
                if (batterySlot.IsPlayerInteractable)
                    isBatterySlotColliding = true;
            }

            if (collider.CompareTag(HangarConstants.BatteryChargerTag) && collider.gameObject.TryGetComponent<BatteryChargePoint>(out var charger))
            {
                if (charger.IsPlayerInteractable)
                    isChargerColliding = true;
            }
        }
        BatteryInteractable.CannotDropBattery = isBatterySlotColliding || isChargerColliding;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
