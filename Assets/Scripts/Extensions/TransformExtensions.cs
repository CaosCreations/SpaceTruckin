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
}
