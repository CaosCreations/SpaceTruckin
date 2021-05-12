using UnityEngine;

public static class HangarConstants
{
    // Licences
    public static int MaximumNumberOfSlots = 24;
    public static int StartingNumberOfSlots = 1;


    // Batteries 
    public static Color ChargedBatteryImageColour = Color.green;
    public static Color DepletedBatteryImageColour = Color.red;
    public static float BatteryEmissionCoefficient = 400f;

    // Spring settings for battery
    public const float BatteryYPosition = 0.4f;
    public const float Spring = 1000f;
    public const float Damper = 0f;
    public const float MinDistance = 0.25f;
    public const float MaxDistance = 0.25f;
    public const float Tolerance = 0f;
    public const bool EnableCollision = true;
    public const float BreakForce = 400f;
    public const bool AutoConfigureConnectedAnchor = false;
    public const float ConnectedAnchorYPosition = 0f;
    public const float distanceBetweenPlayerAndBattery = 0.25f;

    // Battery rigidbody constraints 
    public const RigidbodyConstraints BatteryRigidbodyConstraintsTaken = RigidbodyConstraints.FreezePositionY 
        | RigidbodyConstraints.FreezeRotationX 
        | RigidbodyConstraints.FreezeRotationY 
        | RigidbodyConstraints.FreezeRotationZ;
    
    public const RigidbodyConstraints BatteryRigidbodyConstraintsDropped = RigidbodyConstraints.FreezeRotationX 
        | RigidbodyConstraints.FreezeRotationY 
        | RigidbodyConstraints.FreezeRotationZ;


    // Tags
    public static string BatteryTag = "Battery";
    public static string BatteriesContainerTag = "BatteriesContainer";

}
