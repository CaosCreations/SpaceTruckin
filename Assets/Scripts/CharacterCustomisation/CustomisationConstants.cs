using UnityEngine;

public class CustomisationConstants : MonoBehaviour
{
    // Dimensions 
    public const int CustomisationContainerSpacing = 16;

    // GameObject names 
    public static readonly string CustomisationContainerName = "CustomisationContainer";
    public static readonly string CustomisationOptionName = "CustomisationOption";
    public static readonly string CustomisationImageName = "CustomisationImage";

    // Paths to png files 
    public static readonly string SubDirPath = "Sprites/UI/";
    public static readonly string LeftArrowPath = SubDirPath + "Directional Arrows_0";
    public static readonly string RightArrowPath = SubDirPath + "Direction Arrows_4";
}
