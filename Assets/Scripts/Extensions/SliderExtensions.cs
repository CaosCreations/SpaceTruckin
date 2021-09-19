using UnityEngine.Events;
using UnityEngine.UI;

public static class SliderExtensions
{
    public static void AddOnValueChanged(this Slider self, UnityAction callback)
    {
        self.onValueChanged.RemoveAllListeners();
        self.onValueChanged.AddListener(x => callback());
    }
}
