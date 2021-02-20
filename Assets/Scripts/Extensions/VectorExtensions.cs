using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ApplyScaleFactor(this Vector3 self, float scaleFactor)
    {
        self = new Vector3(self.x * scaleFactor, self.y * scaleFactor, self.z * scaleFactor);
        return self;
    }
}
