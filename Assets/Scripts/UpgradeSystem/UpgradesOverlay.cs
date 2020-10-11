using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesOverlay : MonoBehaviour
{
    private UpgradesGrid upgradesGrid;

    private void Start()
    {
        upgradesGrid = GetComponent<UpgradesGrid>();
    }

    private void OnGUI()
    {
        GUI.DrawTexture(UpgradesConstants.gridOverlayRect, 
            upgradesGrid.SetGridPixels());
    }
}
