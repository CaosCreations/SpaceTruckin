using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static Button AddOnClick(this Button self, UnityAction callback,
        UISoundEffect soundEffect = UISoundEffect.Confirm, bool removeListeners = true)
    {
        if (self == null)
        {
            Debug.LogWarning("Button passed to AddOnClick extension was null");
            return self;
        }

        if (removeListeners)
        {
            self.onClick.RemoveAllListeners();
        }

        self.onClick.AddListener(callback);

        if (UISoundEffectsManager.Instance != null)
        {
            self.onClick.AddListener(() => UISoundEffectsManager.Instance.PlaySoundEffect(soundEffect));
        }
        else
        {
            Debug.LogWarning("Could not add sound fx listener as the manager instance was null.");
        }

        return self;
    }

    public static void SetText(this Button self, string text, FontType fontType = FontType.Button)
    {
        Text buttonText = self.GetComponentInChildren<Text>();

        if (buttonText != null && !string.IsNullOrWhiteSpace(text))
        {
            buttonText.SetText(text, fontType);
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

    public static Button SetActive(this Button self, bool value)
    {
        self.gameObject.SetActive(value);
        return self;
    }
}
