﻿using System.Collections.Generic;
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

    // Input vectors 
    public static readonly Vector3 Vector3UpLeft = new Vector3(-1f, 1f);
    public static readonly Vector3 Vector3DownLeft = new Vector3(-1f, -1f);
    public static readonly Vector3 Vector3DownRight = new Vector3(1f, -1f);
    public static readonly Vector3 Vector3UpRight = new Vector3(1f, 1f);

    // The animation states to be turned on when the movement vector matches
    public static readonly string[] MovementAnimationUpMap = new[] { AnimationUpParameter };
    public static readonly string[] MovementAnimationUpLeftMap = new[] { AnimationUpParameter, AnimationLeftParameter };
    public static readonly string[] MovementAnimationUpRightMap = new[] { AnimationUpParameter, AnimationRightParameter };
    public static readonly string[] MovementAnimationLeftMap = new[] { AnimationLeftParameter };
    public static readonly string[] MovementAnimationDownMap = new[] { AnimationDownParameter };
    public static readonly string[] MovementAnimationDownLeftMap = new[] { AnimationDownParameter, AnimationLeftParameter };
    public static readonly string[] MovementAnimationDownRightMap = new[] { AnimationDownParameter, AnimationRightParameter };
    public static readonly string[] MovementAnimationRightMap = new[] { AnimationRightParameter };

    /// <summary>
    /// The relationships between animator parameter values and the player's movement vector.
    /// </summary>
    public static readonly Dictionary<Vector3, string[]> MovementAnimationMap = new Dictionary<Vector3, string[]>()
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
}