using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCornersPositionTracker : MonoBehaviour
{
    public float GetLeftCornerPosition()
    {
        return transform.position.x - transform.localScale.x / 2;
    }

    public float GetRightCornerPosition()
    {
        return transform.position.x + transform.localScale.x / 2;
    }
}
