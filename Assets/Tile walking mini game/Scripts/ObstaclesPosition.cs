using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesPosition
{
    public int Xposition { get; private set; }
    public int Yposition { get; private set; }

    public ObstaclesPosition(int _Xposition, int _Yposition)
    {
        Xposition = _Xposition;

        Yposition = _Yposition;
    }
}
