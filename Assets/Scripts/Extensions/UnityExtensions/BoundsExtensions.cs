using UnityEngine;

public static class BoundsExtensions
{
    public static Vector3 GetCentralPointAlongLongEdge(this Bounds bounds)
    {
        Vector3 size = bounds.size;
        Vector3 center = bounds.center;

        if (size.x > size.z)
        {
            // Rectangle is aligned with the x-axis
            float centralPointX = center.x + size.x / 2f;
            return new Vector3(centralPointX, center.y, center.z);
        }

        if (size.z > size.x)
        {
            // Rectangle is aligned with the z-axis
            float centralPointZ = center.z + size.z / 2f;
            return new Vector3(center.x, center.y, centralPointZ);
        }
        // Rectangle might be a square or aligned with both axes
        return center;
    }
}