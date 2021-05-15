using System.Collections.Generic;
using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public static KeyCode ActionKey = KeyCode.E;
    public static KeyCode ExitKey = KeyCode.Escape;
    public static KeyCode DropObjectKey = KeyCode.Q;
    public static KeyCode RespawnKey = KeyCode.R;
    public static KeyCode SprintKey = KeyCode.LeftShift;

    public static string PlayerTag = "Player";

    // Movement 
    public const float WalkSpeed = 3f;
    public const float RunSpeed = 7f;
    public const float Gravity = -9.81f;
    public const float KillFloorHeight = -25;
    public static Vector3 PlayerResetPosition = new Vector3(210f, 380f, -247f);

    public static Vector3 Vector3UpLeft = new Vector3(-1f, 1f);
    public static Vector3 Vector3DownLeft = new Vector3(-1f, -1f);
    public static Vector3 Vector3DownRight = new Vector3(1f, -1f);

    public const string AnimationUpParameter = "KeyUp";
    public const string AnimationLeftParameter = "KeyLeft";
    public const string AnimationDownParameter = "KeyDown";
    public const string AnimationRightParameter = "KeyRight";

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// </summary>
    public static Dictionary<Vector3, string> PlayerMovementMap = new Dictionary<Vector3, string>()
    {
        { Vector3.up, AnimationUpParameter },
        { Vector3UpLeft, AnimationUpParameter },
        { Vector3.left, AnimationLeftParameter },
        { Vector3DownLeft, AnimationLeftParameter },
        { Vector3.down, AnimationDownParameter },
        { Vector3DownRight, AnimationDownParameter },
        { Vector3.right, AnimationRightParameter },
        { Vector3.one, AnimationRightParameter }
    };

    // Misc
    public const int MaxPlayerNameLength = 24;

}
