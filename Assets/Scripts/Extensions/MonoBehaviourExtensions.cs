using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void SetActive(this MonoBehaviour self, bool value)
    {
        self.gameObject.SetActive(value);
    }
}