﻿using UnityEngine;

public class UIConstants : MonoBehaviour
{
    public static Color InactiveTabButtonColour = new Color(0.949f, 0.941f, 0.898f); // Off-white 

    public static Vector3 ShipPreviewOffset = new Vector3(-3f, 0f, 0f);
    public static Vector3 ShipPreviewRotationSpeed = new Vector3(0f, 0.15f, 0f);
    public static float ShipPreviewScaleFactor = 0.85f;

    // Layers 
    public static int UILayer = 5;
    public static int UIObjectLayer = 9;

    public static string CloseReportButtonText = "Close";
}
