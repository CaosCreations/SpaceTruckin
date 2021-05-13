using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public const KeyCode ActionKey = KeyCode.E;
    public const KeyCode ExitKey = KeyCode.Escape;
    public const KeyCode DropObjectKey = KeyCode.Q;
    public const KeyCode RespawnKey = KeyCode.R;
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

    public const int MaxPlayerNameLength = 24;

}
