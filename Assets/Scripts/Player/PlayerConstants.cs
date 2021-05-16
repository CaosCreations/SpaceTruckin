using System.Collections.Generic;
using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public const KeyCode ActionKey = KeyCode.E;
    public const KeyCode ExitKey = KeyCode.Escape;
    public const KeyCode DropObjectKey = KeyCode.Q;
    public const KeyCode RespawnKey = KeyCode.R;
    public static KeyCode SprintKey = KeyCode.LeftShift;
    public const KeyCode NextCardKey = KeyCode.RightArrow;
    public const KeyCode CloseCardCycleKey = KeyCode.Return;
    public const KeyCode ChooseNameKey = KeyCode.Return;

    public const string PlayerTag = "Player";

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
    public static string[] MovementAnimationUpMap = new[] { AnimationUpParameter };
    public static string[] MovementAnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static string[] MovementAnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static string[] MovementAnimationLeftMap = new[] { AnimationLeftParameter };
    public static string[] MovementAnimationDownMap = new[] { AnimationDownParameter };
    public static string[] MovementAnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static string[] MovementAnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static string[] MovementAnimationRightMap = new[] { AnimationRightParameter };

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// </summary>
    public static Dictionary<Vector3, string[]> MovementAnimationMap = new Dictionary<Vector3, string[]>()
    {
        { Vector3.up, MovementAnimationUpMap },

        { Vector3UpLeft, MovementAnimationUpLeftMap },

        { Vector3UpRight, MovementAnimationUpRightMap },

        { Vector3.left, MovementAnimationLeftMap },

        { Vector3.down, MovementAnimationDownMap },

        { Vector3DownLeft, MovementAnimationDownLeftMap },

        { Vector3DownRight, MovementAnimationDownRightMap },

        { Vector3.right, MovementAnimationRightMap },
    };

    #endregion

    // Misc
    public const int MaxPlayerNameLength = 24;

}
