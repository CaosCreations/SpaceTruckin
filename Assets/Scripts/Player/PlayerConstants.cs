﻿using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public const KeyCode ActionKey = KeyCode.E;
    public const KeyCode ExitKey = KeyCode.Escape;
    public const KeyCode PauseKey = KeyCode.Escape;
    public const KeyCode RespawnKey = KeyCode.R;
    public const KeyCode RunKey = KeyCode.LeftShift;
    public const KeyCode NextCardKey = KeyCode.RightArrow;
    public const KeyCode CloseCardCycleKey = KeyCode.Return;
    public const KeyCode ChooseNameKey = KeyCode.Return;

    public const string PlayerTag = "Player";

    // Movement 
    public const float WalkSpeed = 3f;
    public const float RunSpeed = 7f;
    public const float Gravity = -9.81f;
    public const float KillFloorHeight = -25;
    public const float RaycastDistance = 10f;
    public const string RaycastIgnoreLayer = "Ignore Raycast";
    public static readonly Vector3 PlayerRespawnPosition = new(209.728f, 380.065f, -252.4218f);
    public static readonly Vector3 PlayerSleepRespawnPosition = new(210.884995f, 380f, -255.25f);
    public static readonly Vector3 PlayerRefugeeCampPosition = new(151.312759f, 380.410004f, -282.76001f);
    public static readonly Vector3 PlayerSpaceportPosition = new(303.287994f, 379.882996f, -283f);
    public static readonly Vector3 PlayerMaintenancePosition = new(202.492996f, 269.5f, -307.501007f);
    public static readonly Vector3 PlayerUlssPosition = new(175.399994f, 380.287292f, -264.440002f);
    public const int EditorTargetFrameRate = 60;

    // Prototyping
    public const KeyCode PrototypingModifier = KeyCode.LeftControl;
    public const KeyCode TerminalShortcut = KeyCode.T;
    public const KeyCode NoticeboardShortcut = KeyCode.Y;
    public const KeyCode FinishTimelineShortcut = KeyCode.F;
    public const KeyCode SpeedUpTimeKey = KeyCode.Q;
    public const KeyCode SkipConvoKey = KeyCode.G;


    // Misc
    public const int MaxPlayerNameLength = 24;

}
