using UnityEngine;
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

    public static void SetActive(this Text self, bool value)
    {
        self.gameObject.SetActive(value);
    }

    public static Text Clear(this Text self)
    {
        return self.SetText(string.Empty);
    }
}
