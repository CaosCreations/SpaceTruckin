using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradesConstants : MonoBehaviour
{
    //public static Vector2 gridOverlayPosition = new Vector2(512f, 512f);
    public static Rect gridOverlayRect = new Rect(256f, 256f, 256f, 256f); 

    // Grid dimensions 
    public static int gridWidth = 16;
    public static int gridHeight = 16;

    public static int cellWidth = 16;
    public static int cellHeight = 16;

    // Grid colours 
    public static Color gridLineColour = Color.black;
    public static Color cellEmptyColour = Color.grey;
    public static Color cellOccupiedColour = Color.blue;
    public static Color cellAccomodatingColour = Color.green;
    public static Color cellUnaccomodatingColour = Color.red;

}
