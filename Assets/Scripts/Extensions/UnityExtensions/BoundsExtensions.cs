using UnityEngine;

public static class BoundsExtensions
{
    public static Vector3 GetCentralPointAlongLongEdge(this Bounds bounds)
    {
        Vector3 size = bounds.size;
        Vector3 center = bounds.center;

        if (size.x > size.z)
        {
            // Rectangle is aligned with the X-axis
            float centralPointX = center.x;
            return new Vector3(centralPointX, center.y, center.z);
        }

        if (size.z > size.x)
        {
            // Rectangle is aligned with the Y-axis
            float centralPointZ = center.z;
            return new Vector3(center.x, center.y, centralPointZ);
        }
        // Rectangle might be a square or aligned with both axes
        return center;
    }
}