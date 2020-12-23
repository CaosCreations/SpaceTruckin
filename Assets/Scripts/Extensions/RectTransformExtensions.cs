using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetAnchors(this RectTransform self, System.ValueTuple<Vector2, Vector2> anchors)
    {
        self.anchorMin = anchors.Item1;
        self.anchorMax = anchors.Item2;
    }

    public static void ResetRect(this RectTransform self)
    {
        // Set Top, Right, Bottom, Left to 0
        self.offsetMin = Vector2.zero;
        self.offsetMax = Vector2.zero;

        // Set Scale to 1 
        self.localScale = Vector2.one;
    }

}
