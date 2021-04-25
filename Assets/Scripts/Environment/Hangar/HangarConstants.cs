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


    // Tags
    public static string BatteryTag = "Battery";
    public static string BatteriesContainerTag = "BatteriesContainer";

}
