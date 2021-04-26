using UnityEngine.Events;
using UnityEngine.UI;

public static class InputFieldExtensions
{
    public static void AddOnValueChanged(this InputField self, UnityAction callback)
    {
        self.onValueChanged.RemoveAllListeners();
        self.onValueChanged.AddListener(delegate { callback(); });
    }
}
