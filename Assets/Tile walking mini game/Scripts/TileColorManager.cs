using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileColorManager : MonoBehaviour
{
    [SerializeField] private Color touchColor;

    [SerializeField] private Color untouchedColor;

    [SerializeField] private Color touchedTwiceColor;

    [SerializeField] private Color obstacleColor;

    public void ChangeTileColorBasedOnStatus(Tile tile)
    {
        switch(tile.TileStatus)
        {
            case TileStatus.Untouched:
                    tile.TileGraphic.color = untouchedColor;
                    break;

            case TileStatus.Touched:
                    tile.TileGraphic.color = touchColor;
                    break;

            case TileStatus.TouchedTwice:
                    tile.TileGraphic.color = touchedTwiceColor;
                    break;

            case TileStatus.Obstacle:
                    tile.TileGraphic.color = obstacleColor;
                    break;
        }
    }
}
