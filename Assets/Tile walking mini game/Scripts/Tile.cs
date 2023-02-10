using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public TileStatus TileStatus;

    public RawImage TileGraphic;

    public RectTransform RectTransform;
}

public enum TileStatus
{
    Obstacle,
    Touched,
    TouchedTwice,
    Untouched
}
