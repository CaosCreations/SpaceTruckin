using UnityEngine;

public class CustomisationConstants : MonoBehaviour
{
    // Dimensions 
    public static int customisationContainerSpacing = 16;
    public static (Vector2, Vector2) saveButtonAnchors = (new Vector2(0.4f, 0.05f), new Vector2(0.6f, 0.3f));

    // GameObject names 
    public static string customisationContainerName = "CustomisationContainer"; 
    public static string customisationOptionName = "CustomisationOption";
    public static string customisationImageName = "CustomisationImage";
    public static string saveButtonName = "SaveButton";
    public static string saveButtonText = "Save Changes";

    // Paths to png files 
    public static string subDirPath = "Sprites/UI/";
    public static string leftArrowPath = subDirPath + "Directional Arrows_0";
    public static string rightArrowPath = subDirPath + "Direction Arrows_4";
}
