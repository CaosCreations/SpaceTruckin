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

    private void HandlePositionShortcuts()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerManager.PlayerMovement.SetPosition(PlayerConstants.PlayerRefugeeCampPosition);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        HandleMenuShortcuts();
        HandlePositionShortcuts();
#endif
    }
}
