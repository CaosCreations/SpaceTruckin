using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCornersPositionPile
{
    public List<CubeCornersPositionTracker> CubeCornersPositionList { get; private set; } = new List<CubeCornersPositionTracker>();

    public void Add(CubeCornersPositionTracker cubeCornersPositionTracker)
    {
        if (CubeCornersPositionList.Count < 2)
        {
            CubeCornersPositionList.Add(cubeCornersPositionTracker);
        }

        else
        {
            CubeCornersPositionList[0] = CubeCornersPositionList[1];
            CubeCornersPositionList[1] = cubeCornersPositionTracker;
        }
    }

    public void ResetPile()
    {
        CubeCornersPositionList.Clear();
    }
}
