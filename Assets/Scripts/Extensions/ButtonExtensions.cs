public static class ButtonExtensions
{
    public static void AddOnClick(this UnityEngine.UI.Button self, UnityEngine.Events.UnityAction callback)
    {
        self.onClick.RemoveAllListeners();
        self.onClick.AddListener(callback);
    }
}
