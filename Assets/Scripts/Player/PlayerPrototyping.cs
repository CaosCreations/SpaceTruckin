using UnityEngine;

/// <summary>To speed up prototyping. Will only be enabled in the editor</summary>
public class PlayerPrototyping : MonoBehaviour
{
    [SerializeField]
    private float fastTime = 5f;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(PlayerConstants.PrototypingModifier))
        {
            // Access menus remotely 
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
            // Speed up time 
            else if (Input.GetKeyDown(PlayerConstants.SpeedUpTimeKey))
            {
                if (Time.timeScale == fastTime)
                {
                    Time.timeScale = 1f;
                    ClockManager.StartClock();
                }
                else
                {
                    ClockManager.StopClock();
                    Time.timeScale = fastTime;
                }
            }
        }
        // Port to station locations 
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerManager.PlayerMovement.SetPosition(PlayerConstants.PlayerRefugeeCampPosition, AnimationConstants.RefugeeCampStateName);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerManager.PlayerMovement.SetPosition(PlayerConstants.PlayerUlssPosition, AnimationConstants.UlssStateName);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerManager.PlayerMovement.SetPosition(PlayerConstants.PlayerSpaceportPosition, AnimationConstants.SpaceportStateName);
            }
        }
#endif
    }
}
