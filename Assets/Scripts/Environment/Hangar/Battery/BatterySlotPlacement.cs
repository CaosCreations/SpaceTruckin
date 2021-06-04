using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySlotPlacement : InteractableObject
{
    [SerializeField] private Transform slot;

    private BatteryInteractable batteryInteractableInSlot;

    public void Update()
    {
        if(!PlayerManager.IsPaused
          && IsPlayerColliding
          && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            if (HangarManager.currentBatteryBeingHeld.BatteryInteractable != null
                && batteryInteractableInSlot == null)
            {
                batteryInteractableInSlot = HangarManager.currentBatteryBeingHeld.BatteryInteractable;
                
                /// <summary>
                /// When placing the battery in a slot, we deactivate its wrapper's collider
                /// We do so that the battery system and the battery slot placement don't interfere with each other
                /// Once placed, we want the player to pick up the battery when being within the slot's collider
                ///  </summary>
                batteryInteractableInSlot.Collider.enabled = false;
                batteryInteractableInSlot.PlaceBatteryInSlot(slot);
            }

            // Take battery
            else if(BatteryInteractable.PlayerIsHoldingABattery == false && batteryInteractableInSlot != null)
            {
                batteryInteractableInSlot.TakeBattery();
                batteryInteractableInSlot.Collider.enabled = true;
                batteryInteractableInSlot = null;
            }  
        }
    }

}
