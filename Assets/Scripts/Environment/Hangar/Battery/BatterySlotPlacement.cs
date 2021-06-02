using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySlotPlacement : InteractableObject
{
    public Transform Slot;

    private BatteryInteractable batteryInteractableInSlot;

    public void Update()
    {
        if(!PlayerManager.IsPaused
          && IsPlayerColliding
          && Input.GetKeyDown(PlayerConstants.ActionKey))
        {
            // Place battery
            if(HangarManager.currentBatteryBeingHeld != null && batteryInteractableInSlot == null)
            {
                batteryInteractableInSlot = HangarManager.currentBatteryBeingHeld.BatteryInteractable;
                batteryInteractableInSlot.PlaceBatteryInSlot(Slot);
            }

            // Take battery
            else if(HangarManager.currentBatteryBeingHeld == null && batteryInteractableInSlot != null)
            {
                batteryInteractableInSlot.TakeBattery();
                batteryInteractableInSlot = null;
            }  
        }
    }

}
