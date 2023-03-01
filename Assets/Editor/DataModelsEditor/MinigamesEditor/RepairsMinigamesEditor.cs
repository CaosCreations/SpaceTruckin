using UnityEditor;
using UnityEngine;

public class RepairsMinigamesEditor : MonoBehaviour
{
    //[MenuItem("Space Truckin/Minigames/Init Minigame")]
    private static void InitMinigameSession() => InitMinigame();

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
