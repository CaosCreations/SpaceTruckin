public class BatterySlotHoldingLocation : BatteryHoldingLocation
{
    private BatterySlot batterySlot;

    private void Start()
    {
        batterySlot = interactableObjectWithSlot as BatterySlot;
    }

    protected override bool IsAbleToPutInSlot => base.IsAbleToPutInSlot && batterySlot.CanTransferEnergy();
}
