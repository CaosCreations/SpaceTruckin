using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesGrid : MonoBehaviour
{
    public Texture2D texture;

    private void Start()
    {
        texture = new Texture2D(UpgradesConstants.gridWidth, UpgradesConstants.gridHeight);
        //GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    // Parameter will be based on the current state of the cell 
    public void SetCellPixels(/*Color innerColour*/)
    {
        for (int y = 0; y < UpgradesConstants.cellHeight; y++)
        {
            for (int x = 0; x < UpgradesConstants.cellWidth; x++)
            {
                // Simplify this condition 
                if (y == 0 || y == UpgradesConstants.cellHeight - 1 || 
                    x == 0 || x == UpgradesConstants.cellWidth - 1)
                {
                    texture.SetPixel(x, y, UpgradesConstants.gridLineColour);
                }
                else
                {
                    texture.SetPixel(x, y, UpgradesConstants.cellEmptyColour/*innerColour*/);
                }
            }
            texture.Apply();
        }
    }

    public Texture2D SetGridPixels()
    {
        for (int y = 0; y < UpgradesConstants.gridHeight - 1; y++)
        {
            for (int x = 0; x < UpgradesConstants.gridWidth - 1; x++)
            {
                SetCellPixels();
            }
        }
        return texture; 
    }
}
