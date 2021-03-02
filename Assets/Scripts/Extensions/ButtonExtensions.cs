using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static Button AddOnClick(this Button self, UnityAction callback, SoundEffect soundEffect = SoundEffect.Button)
    {
        self.onClick.RemoveAllListeners();
        self.onClick.AddListener(callback);
        self.onClick.AddListener(() => SoundEffectsManager.PlaySoundEffect(soundEffect));
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
}
