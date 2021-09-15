using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyDirectChildren(this Transform self)
    {
        foreach (Transform child in self)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void DisableDirectChildren(this Transform self)
    {
        foreach (Transform child in self)
        {
            child.gameObject.SetActive(false);
        }
    }
}
