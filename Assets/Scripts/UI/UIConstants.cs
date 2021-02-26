using UnityEngine;

public class UIConstants : MonoBehaviour
{
    public static Vector3 ShipPreviewOffset = new Vector3(-3f, 0f, 0f);
    public static Vector3 ShipPreviewRotationSpeed = new Vector3(0f, 0.15f, 0f);
    public static float ShipPreviewScaleFactor = 0.85f;

    // Layers 
    public static int UILayer = 5;
    public static int UIObjectLayer = 9;

    // Colour palette
    public static Color OffWhite = new Color(0.949f, 0.941f, 0.898f);
    public static Color LightGrey = new Color(184, 181, 185);
    public static Color MediumGrey = new Color(134, 129, 136);
    public static Color DarkGrey = new Color(100, 99, 101);

    public static Color InactiveTabButtonColour = OffWhite;
}
