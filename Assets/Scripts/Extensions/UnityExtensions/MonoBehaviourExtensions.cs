using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static bool IsActive(this MonoBehaviour self)
    {
        return self.gameObject != null && self.gameObject.activeSelf;
    }

    public static void SetActive(this MonoBehaviour self, bool value)
    {
        if (self == null)
        {
            Debug.LogError("MonoBehaviour was null when trying to set active to: " + value);
            return;
        }
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

    public static void StartCoroutine(this MonoBehaviour self, IEnumerator routine)
    {
        self.StartCoroutine(routine);
    }
}
