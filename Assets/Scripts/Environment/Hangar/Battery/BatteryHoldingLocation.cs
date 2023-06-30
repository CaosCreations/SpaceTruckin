using UnityEngine;

public class BatteryHoldingLocation : MonoBehaviour
{
    [SerializeField] private Transform slotTransform;

    private BatteryInteractable batteryInteractableInSlot;

    // This script is placed on an InteractableObject script.
    // We need it to check for collisions with the player.
    [SerializeField] protected InteractableObject interactableObjectWithSlot;

    public virtual void Update()
    {
        if (!IsAttemptingToInteract())
        {
            return;
        }

        if (IsAbleToPutInSlot())
        {
            PutInSlot();
        }
        else if (IsAbleToTakeFromSlot())
        {
            TakeFromSlot();
        }
    }

    protected bool IsAttemptingToInteract()
    {
        return !PlayerManager.IsPaused
            && Input.GetKeyDown(PlayerConstants.ActionKey)
            && interactableObjectWithSlot.IsPlayerInteractable;
    }

    protected virtual bool IsAbleToPutInSlot()
    {
        return HangarManager.CurrentBatteryBeingHeld != null
            && HangarManager.CurrentBatteryBeingHeld.BatteryInteractable != null
            && batteryInteractableInSlot == null;
    }

    protected bool IsAbleToTakeFromSlot()
    {
        return !BatteryInteractable.IsPlayerHoldingABattery
            && batteryInteractableInSlot != null;
    }

    /// <summary>
    /// When placing the battery in a slot, we deactivate its wrapper's collider
    /// We do so that the battery system and the battery slot placement don't interfere with each other
    /// Once placed, we want the player to pick up the battery when being within the slot's collider
    ///  </summary>
    public void PutInSlot()
    {
        batteryInteractableInSlot = HangarManager.CurrentBatteryBeingHeld.BatteryInteractable;
        //batteryInteractableInSlot.Collider.enabled = false;
        batteryInteractableInSlot.PlaceBatteryInSlot(slotTransform);
    }

    protected void TakeFromSlot()
    {
        batteryInteractableInSlot.TakeBattery();
        batteryInteractableInSlot.Collider.enabled = true;
        batteryInteractableInSlot = null;
    }
}
