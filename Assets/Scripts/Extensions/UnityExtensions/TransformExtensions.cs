using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyDirectChildren(this Transform self)
    {
        foreach (Transform child in self)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static void SetDirectChildrenActive(this Transform self, bool active)
    {
        foreach (Transform child in self)
        {
            child.gameObject.SetActive(active);
        }
    }
}
