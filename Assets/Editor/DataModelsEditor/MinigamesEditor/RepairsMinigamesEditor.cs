using UnityEditor;
using UnityEngine;

public class RepairsMinigamesEditor : MonoBehaviour
{
    //[MenuItem("Space Truckin/Minigames/Init Minigame")]
    [System.Obsolete]
#pragma warning disable IDE0051 // Remove unused private members
    private static void InitMinigameSession() => InitMinigame();
#pragma warning restore IDE0051 // Remove unused private members

    [System.Obsolete]
    private static void InitMinigame()
    {
        var ship = HangarEditor.DockShipAtAvailableNode(out int hangarNode);
        UIManager.HangarNode = hangarNode;
        UIManager.ShowCanvas(UICanvasType.Hangar, true);

        var hangarUI = FindObjectOfType<HangarNodeUI>();
        hangarUI.ShipToInspect = ship;
        hangarUI.SwitchPanel(HangarNodeUI.HangarPanel.Repair);
    }
}
