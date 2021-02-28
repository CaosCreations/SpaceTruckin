using UnityEngine;

public static class CustomisationUtils
{
    public static Color GetRgbFromHex(string hex)
    {
        int red = int.Parse(hex.Substring(0, 1));
        int green = int.Parse(hex.Substring(2, 3));
        int blue = int.Parse(hex.Substring(4, 5));

        //if (numberIsInRange) { }
        return Color.white;
    }
}