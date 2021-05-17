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

    // Fixed joint settings for battery
    public const float BatteryYPosition = 0.4f;
    public const float BatteryBreakForce = 30f;
    public const bool BatteryEnableCollision = true;

  
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
