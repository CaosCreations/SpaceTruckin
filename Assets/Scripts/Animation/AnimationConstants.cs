using System.Collections.Generic;
using UnityEngine;

public static class AnimationConstants
{
    #region Player Animation

    // State machine parameters 
    public const string AnimationUpParameter = "KeyUp";
    public const string AnimationLeftParameter = "KeyLeft";
    public const string AnimationDownParameter = "KeyDown";
    public const string AnimationRightParameter = "KeyRight";
    public const string AnimationRunParameter = "RUN";
    public const string AnimationBatteryGrabbingParameter = "batteryGrabbing";

    public const string NpcWalkParameter = "Walk";
    public const string NpcIdleParameterPrefix = "Idle";
    public const string NpcMorningParameter = "Morning";
    public const string NpcEveningParameter = "Evening";

    public const string BabyUpParameter = "WalkUpHdBaby";
    public const string BabyLeftParameter = "WalkLeftHdBaby";
    public const string BabyDownParameter = "walkDownHdBaby";
    public const string BabyRightParameter = "WalkRightHdBaby";

    // Input vectors 
    public static readonly Vector3 Vector3UpLeft = new(-1f, 1f);
    public static readonly Vector3 Vector3DownLeft = new(-1f, -1f);
    public static readonly Vector3 Vector3DownRight = new(1f, -1f);
    public static readonly Vector3 Vector3UpRight = new(1f, 1f);

    // The animation states to be turned on when the movement vector matches
    public static readonly string[] MovementAnimationUpMap = new[] { AnimationUpParameter };
    public static readonly string[] MovementAnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static readonly string[] MovementAnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static readonly string[] MovementAnimationLeftMap = new[] { AnimationLeftParameter };
    public static readonly string[] MovementAnimationDownMap = new[] { AnimationDownParameter };
    public static readonly string[] MovementAnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static readonly string[] MovementAnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static readonly string[] MovementAnimationRightMap = new[] { AnimationRightParameter };

    public static readonly string[] BabyAnimationUpMap = new[] { BabyUpParameter };
    public static readonly string[] BabyAnimationUpLeftMap = new[] { BabyUpParameter, BabyLeftParameter };
    public static readonly string[] BabyAnimationUpRightMap = new[] { BabyUpParameter, BabyRightParameter };
    public static readonly string[] BabyAnimationLeftMap = new[] { BabyLeftParameter };
    public static readonly string[] BabyAnimationDownMap = new[] { BabyDownParameter };
    public static readonly string[] BabyAnimationDownLeftMap = new[] { BabyDownParameter, BabyLeftParameter };
    public static readonly string[] BabyAnimationDownRightMap = new[] { BabyDownParameter, BabyRightParameter };
    public static readonly string[] BabyAnimationRightMap = new[] { BabyRightParameter };

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// </summary>
    public static readonly Dictionary<Vector3, string[]> PlayerMovementAnimationMap = new()
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

    public static readonly Dictionary<Vector3, string[]> BabyMovementAnimationMap = new()
    {
        { Vector3.up, BabyAnimationUpMap },
        { Vector3UpLeft, BabyAnimationUpLeftMap },
        { Vector3UpRight, BabyAnimationUpRightMap },
        { Vector3.left, BabyAnimationLeftMap },
        { Vector3.down, BabyAnimationDownMap },
        { Vector3DownLeft, BabyAnimationDownLeftMap },
        { Vector3DownRight, BabyAnimationDownRightMap },
        { Vector3.right, BabyAnimationRightMap },
    };

    public const string Player1ControllerName = "SprPlayernewAnim";
    public const string Player2ControllerName = "SprPlayer2newAnim";

    /// <summary>
    ///     P1 to P2 mappings of animation clip names.
    /// </summary>
    public static readonly Dictionary<string, string> TimelineAnimationClipNameMappings = new Dictionary<string, string>
    {
        { "WalkLeft", "player2_walkLeft" },
        { "StandDownP", "player2_IdleDown" },
        { "StandRightP", "player2_IdleRight" },
        { "StandUpP", "player2_IdleUp" },
        { "StandLefttP", "player2_IdleLeft" },
        { "RunDown", "player2_runDown" },
        { "RunLeft", "player2_runLeft" },
        { "RunUP", "player2_runUp" }
    };
    #endregion
}
