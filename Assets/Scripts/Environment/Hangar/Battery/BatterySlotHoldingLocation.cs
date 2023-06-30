using Events;

/// <summary>
/// With logic that uses whether or not the ship can receive charge.  
/// </summary>
public class BatterySlotHoldingLocation : BatteryHoldingLocation
{
    private BatterySlot batterySlot;

    private void Start()
    {
        batterySlot = interactableObjectWithSlot as BatterySlot;
        SingletonManager.EventService.Add<OnShipChargedEvent>(OnShipChargedHandler);
    }

    public override void Update()
    {
        if (!IsAttemptingToInteract())
        {
            return;
        }

        if (IsAbleToTakeFromSlot())
        {
            TakeFromSlot();
        }
    }

    //protected override bool IsAbleToPutInSlot => base.IsAbleToPutInSlot && batterySlot.CanTransferEnergy();

    private void OnShipChargedHandler(OnShipChargedEvent evt)
    {
        if (batterySlot.Node != evt.Node)
            return;

        //PutInSlot();
    }
}
