using System.Collections.Generic;
using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public const KeyCode ActionKey = KeyCode.E;
    public const KeyCode ExitKey = KeyCode.Escape;
    public const KeyCode PauseKey = KeyCode.Escape;
    public const KeyCode DropObjectKey = KeyCode.Q;
    public const KeyCode RespawnKey = KeyCode.R;
    public const KeyCode SprintKey = KeyCode.LeftShift;
    public const KeyCode NextCardKey = KeyCode.RightArrow;
    public const KeyCode CloseCardCycleKey = KeyCode.Return;
    public const KeyCode ChooseNameKey = KeyCode.Return;

    public const string PlayerTag = "Player";

    // Movement 
    public const float WalkSpeed = 3f;
    public const float RunSpeed = 7f;
    public const float Gravity = -9.81f;
    public const float KillFloorHeight = -25;
    public static readonly Vector3 PlayerResetPosition = new Vector3(209.728f, 380.065f, -252.4218f);
    public const int EditorTargetFramerate = 60;

    // Prototyping
    public const KeyCode PrototypingModifier = KeyCode.LeftControl;
    public const KeyCode TerminalShortcut = KeyCode.T;
    public const KeyCode NoticeboardShortcut = KeyCode.G;
    public const KeyCode FinishTimelineShortcut = KeyCode.F;

    public static readonly List<KeyCode> HangarNodeShortcuts = new List<KeyCode>()
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6
    };

    // Misc
    public const int MaxPlayerNameLength = 24;

}
