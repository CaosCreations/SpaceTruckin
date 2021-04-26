using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void SetActive(this MonoBehaviour self, bool value)
    {
        self.gameObject.SetActive(value);
    }

    public static void DestroyIfExists(this MonoBehaviour self)
    {
        if (self != null)
        {
            self.gameObject.DestroyIfExists();
        }
    }

    public static bool HasChildOfType<T>(this MonoBehaviour self) where T : MonoBehaviour
    {
        return self.GetComponentInChildren<T>() != null;
    }
}