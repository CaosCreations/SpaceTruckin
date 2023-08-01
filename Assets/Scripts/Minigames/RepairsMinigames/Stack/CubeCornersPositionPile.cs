using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCornersPositionPile
{
    public CubeCornersPositionTracker TopCube { get; private set; }

    public CubeCornersPositionTracker BottomCube { get; private set; }

    public void Add(CubeCornersPositionTracker cubeToAdd)
    {
        if (BottomCube == null)
            BottomCube = cubeToAdd;

        else if (TopCube == null)
            TopCube = cubeToAdd;

        else
        {
            BottomCube = TopCube;
            TopCube = cubeToAdd;
        }
    }

    public void ResetPile()
    {
        TopCube = BottomCube = null;
    }
}
