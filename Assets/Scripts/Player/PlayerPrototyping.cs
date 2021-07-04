using System.Linq;
using UnityEngine;

public class PlayerPrototyping : MonoBehaviour
{
    private void HandleShortcuts()
    {
        if (!Input.GetKey(PlayerConstants.PrototypingModifier))
        {
            return;
        }

        if (Input.GetKeyDown(PlayerConstants.TerminalShortcut))
        {
            UIManager.ShowCanvas(UICanvasType.Terminal);
        }
        else if (Input.GetKeyDown(PlayerConstants.NoticeboardShortcut))
        {
            UIManager.ShowCanvas(UICanvasType.NoticeBoard);
        }
    }

    private void Update()
    {
        #if UNITY_EDITOR
            HandleShortcuts();
        #endif
    }

    public int GetHangarNodeFromKey()
    {
        int node = -1;

        foreach (var keyCode in PlayerConstants.HangarNodeShortcuts)
        {
            if (Input.GetKeyDown(keyCode))
            {
                char keyChar = keyCode.ToString().Last();
                node = keyChar - '0';
            }
        }
        return node;
    }
}
