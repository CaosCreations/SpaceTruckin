using UnityEngine;
using UnityEngine.UI;

public static class TextExtensions
{
    public static void SetDefaultFont(this Text self)
    {
        self.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }

    public static string SetText(this Text self, string value, Font font = null)
    {
        if (font != null && self.font != font)
        {
            self.font = font;
        }
        return self.text = value.InsertNewLines();
    }
}
