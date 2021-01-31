using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static void AddOnClick(this Button self, UnityEngine.Events.UnityAction callback)
    {
        self.onClick.RemoveAllListeners();
        self.onClick.AddListener(callback);
    }

    public static void SetColour(this Button self, Color newColour)
    {
        self.GetComponent<Image>().color = newColour;
    }
}
