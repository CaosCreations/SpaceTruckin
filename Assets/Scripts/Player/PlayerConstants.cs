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

    // Input vectors 
    public static Vector3 Vector3UpLeft = new Vector3(-1f, 1f);
    public static Vector3 Vector3DownLeft = new Vector3(-1f, -1f);
    public static Vector3 Vector3DownRight = new Vector3(1f, -1f);
    public static Vector3 Vector3UpRight = new Vector3(1f, 1f);

    #region Player Movement Animation
    // State machine parameters 
    public const string AnimationUpParameter = "KeyUp";
    public const string AnimationLeftParameter = "KeyLeft";
    public const string AnimationDownParameter = "KeyDown";
    public const string AnimationRightParameter = "KeyRight";
    public const string AnimationRunParameter = "RUN";

    // The animation states to be turned on when the movement vector matches
    public static string[] ActiveAnimationUpMap = new[] { AnimationUpParameter };
    public static string[] ActiveAnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static string[] ActiveAnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static string[] ActiveAnimationLeftMap = new[] { AnimationLeftParameter };
    public static string[] ActiveAnimationDownMap = new[] { AnimationDownParameter };
    public static string[] ActiveAnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static string[] ActiveAnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static string[] ActiveAnimationRightMap = new[] { AnimationRightParameter };

    // The animation states to be turned off when the movement vector matches
    public static string[] InactiveAnimationUpMap = new[] { AnimationDownParameter };
    public static string[] InactiveAnimationUpLeftMap = new[] { AnimationDownParameter };
    public static string[] InactiveAnimationUpRightMap = new[] { AnimationDownParameter };
    public static string[] InactiveAnimationLeftMap = new[] { AnimationDownParameter };
    public static string[] InactiveAnimationDownMap = new[] { AnimationUpParameter };
    public static string[] InactiveAnimationDownLeftMap = new[] { AnimationUpParameter };
    public static string[] InactiveAnimationDownRightMap = new[] { AnimationUpParameter };
    public static string[] InactiveAnimationRightMap = new[] { AnimationDownParameter };

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// Supports multiple parameters being mapped to each possible movement vector value. 
    /// </summary>
    public static Dictionary<Vector3,
        (string[] activeParams, string[] inactiveParams)> PlayerMovementMap = new Dictionary<Vector3, (string[], string[])>()
    {
        { Vector3.up, (ActiveAnimationUpMap, InactiveAnimationUpMap) },

        { Vector3UpLeft, (ActiveAnimationUpLeftMap, InactiveAnimationUpLeftMap) },

        { Vector3UpRight, (ActiveAnimationUpRightMap, InactiveAnimationUpRightMap) },

        { Vector3.left, (ActiveAnimationLeftMap, InactiveAnimationLeftMap) },

        { Vector3.down, (ActiveAnimationDownMap, InactiveAnimationDownMap) },

        { Vector3DownLeft, (ActiveAnimationDownLeftMap, InactiveAnimationDownLeftMap) },

        { Vector3DownRight, (ActiveAnimationDownRightMap, InactiveAnimationDownRightMap) },

        { Vector3.right, (ActiveAnimationRightMap, InactiveAnimationRightMap) }
    };
    #endregion

    // Misc
    public const int MaxPlayerNameLength = 24;

}
