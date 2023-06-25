using UnityEngine;

public static class HangarConstants
{
    // Licences
    public const int MaximumNumberOfSlots = 24;
    public const int StartingNumberOfSlots = 1;


    // Batteries 
    public static readonly Color ChargedBatteryImageColour = Color.green;
    public static readonly Color DepletedBatteryImageColour = Color.red;
    public const float BatteryEmissionCoefficient = 400f;

    // Fixed joint settings for battery
    public const float BatteryYPosition = 0.5f;

    public const bool BatteryEnableCollision = false;

    public const string BatteryLayer = "Battery";
    public const string BatteryChargerLayer = "BatteryChargePoint";

    // Battery rigidbody constraints 
    public const RigidbodyConstraints BatteryRigidbodyConstraintsTaken = RigidbodyConstraints.FreezePositionY
        | RigidbodyConstraints.FreezeRotationX
        | RigidbodyConstraints.FreezeRotationY
        | RigidbodyConstraints.FreezeRotationZ;

    public const RigidbodyConstraints BatteryRigidbodyConstraintsDropped = RigidbodyConstraints.FreezeRotationX
        | RigidbodyConstraints.FreezeRotationY
        | RigidbodyConstraints.FreezeRotationZ;


    // Tags
    public const string BatteryTag = "Battery";
    public const string BatteriesContainerTag = "BatteriesContainer";
    public const string BatteryExitColliderTag = "BatteryExit";
    public const string BatteryChargerTag = "BatteryChargePoint";
    public const string BatterySlotTag = "BatterySlot";
}
