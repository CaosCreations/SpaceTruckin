﻿using UnityEngine;
using UnityEngine.UI;

public static class TextExtensions
{
    public static void SetDefaultFont(this Text self)
    {
        self.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    }

    public static Text SetText(this Text self, string value, FontType fontType = FontType.Paragraph)
    {
        self.font = FontManager.Instance.GetFontByType(fontType);
        self.text = value.InsertNewLines();
        return self;
    }
}
