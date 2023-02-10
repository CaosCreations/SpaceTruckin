using System.Linq;
using UnityEngine;

/// <summary>To speed up prototyping. Will only be enabled in the editor</summary>
public class PlayerPrototyping : MonoBehaviour
{
    private void HandleMenuShortcuts()
    {
        if (!Input.GetKey(PlayerConstants.PrototypingModifier))
        {
            return;
        }

        // Allow accessing menus remotely 
        if (Input.GetKeyDown(PlayerConstants.TerminalShortcut))
        {
            UIManager.ToggleCanvas(UICanvasType.Terminal);
        }
        else if (Input.GetKeyDown(PlayerConstants.NoticeboardShortcut))
        {
            UIManager.ToggleCanvas(UICanvasType.NoticeBoard);
        }
        else if (Input.GetKeyDown(PlayerConstants.FinishTimelineShortcut))
        {
            TimelineManager.Instance.FinishCurrentTimeline();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMenuShortcuts();
#endif
    }

    public int GetHangarNodeFromKey()
    {
        int node = -1;

        foreach (var keyCode in PlayerConstants.HangarNodeShortcuts)
        {
            if (Input.GetKeyDown(keyCode))
            {
                // Convert the numeric portion of the KeyCode string to an int 
                char keyChar = keyCode.ToString().Last();
                node = keyChar - '0';
            }
        }
        return node;
    }
}
