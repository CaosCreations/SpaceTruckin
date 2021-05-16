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

    #region Movement Animation
    // State machine parameters 
    public const string AnimationUpParameter = "KeyUp";
    public const string AnimationLeftParameter = "KeyLeft";
    public const string AnimationDownParameter = "KeyDown";
    public const string AnimationRightParameter = "KeyRight";
    public const string AnimationRunParameter = "RUN";

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// </summary>
    public static Dictionary<Vector3, string> PlayerMovementMap = new Dictionary<Vector3, string>()
    {
        { Vector3.up, AnimationUpParameter },
        { Vector3UpLeft, AnimationUpParameter },
        { Vector3UpRight, AnimationUpParameter },
        { Vector3.left, AnimationLeftParameter },
        { Vector3.down, AnimationDownParameter },
        { Vector3DownLeft, AnimationDownParameter },
        { Vector3DownRight, AnimationDownParameter },
        { Vector3.right, AnimationRightParameter },
    };

    public static string[] AnimationUpMap = new[] { AnimationUpParameter };
    public static string[] AnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static string[] AnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static string[] AnimationLeftMap = new[] { AnimationLeftParameter };
    public static string[] AnimationDownMap = new[] { AnimationDownParameter };
    public static string[] AnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static string[] AnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static string[] AnimationRightMap = new[] { AnimationRightParameter };

    /// <summary>
    /// Animation state map that supports multiple animator parameters to be mapped to movement vectors 
    /// </summary>
    public static Dictionary<Vector3, string[]> PlayerMovementArrayMap = new Dictionary<Vector3, string[]>()
    {
        { Vector3.up, AnimationUpLeftMap },
        { Vector3UpLeft, AnimationUpLeftMap },
        { Vector3UpRight, AnimationUpRightMap},
        { Vector3.left, AnimationLeftMap},
        { Vector3.down, AnimationDownMap },
        { Vector3DownLeft, AnimationDownLeftMap},
        { Vector3DownRight, AnimationDownRightMap },
        { Vector3.right, AnimationRightMap }
    };

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
    public static string[] InactiveAnimationUpMap = new[] { AnimationUpParameter };
    public static string[] InactiveAnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static string[] InactiveAnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static string[] InactiveAnimationLeftMap = new[] { AnimationLeftParameter };
    public static string[] InactiveAnimationDownMap = new[] { AnimationDownParameter };
    public static string[] InactiveAnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static string[] InactiveAnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static string[] InactiveAnimationRightMap = new[] { AnimationRightParameter };

    /// <summary>
    /// Animation state map that supports multiple animator parameters to be mapped to movement vectors 
    /// </summary>
    public static Dictionary<
        Vector3,
        (string[] activeParams, string[] inactiveParams)> PlayerMovementOnAndOffMap = new Dictionary<Vector3, (string[], string[])>()
    {
        { Vector3.up, (ActiveAnimationUpLeftMap, InactiveAnimationUpMap) },
        { Vector3UpLeft, (ActiveAnimationUpLeftMap, InactiveAnimationUpMap) },
        { Vector3UpRight, (ActiveAnimationUpRightMap, InactiveAnimationUpMap) },
        { Vector3.left, (ActiveAnimationLeftMap, InactiveAnimationUpMap) },
        { Vector3.down, (ActiveAnimationDownMap, InactiveAnimationUpMap) },
        { Vector3DownLeft, (ActiveAnimationDownLeftMap, InactiveAnimationUpMap) },
        { Vector3DownRight, (ActiveAnimationDownRightMap, InactiveAnimationUpMap) },
        { Vector3.right, (ActiveAnimationRightMap, InactiveAnimationUpMap) }
    };
    #endregion

    // Misc
    public const int MaxPlayerNameLength = 24;

}
