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

    #region Camera Animation

    // Camera switches animation parameter triggers
    public const string DefaultSwitchTriggerParameter = "DefaultSwitchTrigger";
    public const string WorkShopSwitchTriggerParameter = "WorkShopSwitchTrigger";
    public const string DinnerSwitchTriggerParameter = "DinnerSwitchTrigger";
    public const string BarSwitchTriggerParameter = "BarSwitchTrigger";
    public const string YogaSwitchTriggerParameter = "YogaSwitchTrigger";
    public const string HangarEdgeSwitchTriggerParameter = "HangarEdgeSwitchTrigger";
    public const string OfficeLoungetSwitchTriggerParameter = "OfficeLoungeSwitchTrigger";
    public const string HangarSwitchTriggerParameter = "HangarSwitchTrigger";
    public const string WashingStablishmentSwitchTriggerParameter = "WashingStablishmentSwitchTrigger";
    public const string AbbandonedTankTriggerParameter = "AbbandonedTankSwitchTrigger";
    public const string SpacePortSwitchTriggerParameter = "SpacePortSwitchTrigger";
    public const string RefugeCampISwitchTriggerParameter = "RefugeCampISwitchTrigger";
    public const string AbbandonedStorageRoomSwitchTriggerParameter = "AbbandonedStorageRoomSwitchTrigger";
    public const string StationMainOfiiceSwitchTriggerParameter = "StationMainOfiiceSwitchTrigger";
    public const string AbandonedTankSwitchTriggerParameter = "AbandonedTankSwitchTrigger";
    public const string NorthEastCorridorTriggerParameter = "NorthEastCorridorSwitchTrigger";
    public const string PPHQTriggerParameter = "PPHQSwitchTrigger";
    public const string RefugeUnd01SwitchTriggerParameter = "RefugeUnd01SwitchTrigger";
    public const string RefugeUnd02SwitchTriggerParameter = "RefugeUnd02SwitchTrigger";
    public const string RefugeUnd03SwitchTriggerParameter = "RefugeUnd03SwitchTrigger";

    #endregion
}
