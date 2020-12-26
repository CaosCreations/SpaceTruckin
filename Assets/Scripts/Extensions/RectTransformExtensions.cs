using UnityEngine;

public enum Side
{
    Top = 0, Right = 1, Bottom = 2, Left = 3
}

public static class RectTransformExtensions
{
    public static void SetAnchors(this RectTransform self, System.ValueTuple<Vector2, Vector2> anchors)
    {
        self.anchorMin = anchors.Item1;
        self.anchorMax = anchors.Item2;
    }

    public static void Stretch(this RectTransform self)
    {
        self.anchorMin = Vector2.zero;
        self.anchorMax = Vector2.one;
    }

    public static void Reset(this RectTransform self)
    {
        // Set Top, Right, Bottom, Left to 0
        self.offsetMin = Vector2.zero;
        self.offsetMax = Vector2.zero;

        // Set Scale to 1 
        self.localScale = Vector2.one;
    }

    public static void SetPadding(this RectTransform self, Side side, float value)
    {
        switch (side)
        {
            case Side.Top:
                self.offsetMax = new Vector2(self.offsetMax.x, -value);
                break;
            case Side.Right:
                self.offsetMax = new Vector2(-value, self.offsetMax.y);
                break;
            case Side.Bottom:
                self.offsetMin = new Vector2(self.offsetMax.x, value);
                break;
            case Side.Left:
                self.offsetMin = new Vector2(value, self.offsetMin.y);
                break;
        }
    }
}
