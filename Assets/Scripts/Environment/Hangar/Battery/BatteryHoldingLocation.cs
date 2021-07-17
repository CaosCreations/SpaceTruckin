using UnityEngine;

public class BatteryHoldingLocation : MonoBehaviour
{
    [SerializeField] private Transform slot;

    private BatteryInteractable batteryInteractableInSlot;

    // This script is placed on an InteractableObject script
    // We need it to check for collisions with the player
    [SerializeField] private InteractableObject interactableObjectWithSlot;

    public void Update()
    {
        if (!PlayerManager.IsPaused
          && interactableObjectWithSlot.IsPlayerColliding
          && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            if (HangarManager.CurrentBatteryBeingHeld != null
                && HangarManager.CurrentBatteryBeingHeld.BatteryInteractable != null
                && batteryInteractableInSlot == null)
            {
                batteryInteractableInSlot = HangarManager.CurrentBatteryBeingHeld.BatteryInteractable;

                /// <summary>
                /// When placing the battery in a slot, we deactivate its wrapper's collider
                /// We do so that the battery system and the battery slot placement don't interfere with each other
                /// Once placed, we want the player to pick up the battery when being within the slot's collider
                ///  </summary>
                batteryInteractableInSlot.Collider.enabled = false;
                batteryInteractableInSlot.PlaceBatteryInSlot(slot);
            }

            else if (!BatteryInteractable.PlayerIsHoldingABattery && batteryInteractableInSlot != null)
            {
                batteryInteractableInSlot.TakeBattery();
                batteryInteractableInSlot.Collider.enabled = true;
                batteryInteractableInSlot = null;
            }
        }
    }
}
