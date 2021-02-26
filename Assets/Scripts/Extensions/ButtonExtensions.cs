using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static Button AddOnClick(this Button self, UnityAction callback)
    {
        self.onClick.RemoveAllListeners();
        self.onClick.AddListener(callback);
        return self;
    }

    public static void SetText(this Button self, string text)
    {
        Text buttonText = self.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.SetText(text, FontType.Button);
        }
    }

    public static void SetColour(this Button self, Color newColour)
    {
        Image image = self.GetComponent<Image>();
        if (image != null)
        {
            image.color = newColour;
        }
    }

    public static Button SetInteractable(this Button self, bool value)
    {
        self.interactable = value;
        return self;
    }
}
