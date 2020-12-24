using UnityEngine;
using UnityEngine.UI;

public static class TextExtensions
{
    public static void SetDefaultFont(this Text self)
    {
        self.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }
}
