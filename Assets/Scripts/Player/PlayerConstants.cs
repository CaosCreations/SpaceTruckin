using UnityEngine;

public class PlayerConstants : MonoBehaviour
{
    // Keybinds
    public static KeyCode ActionKey = KeyCode.E;
    public static KeyCode ExitKey = KeyCode.Escape;
    public static KeyCode DropObjectKey = KeyCode.Q;
    public static KeyCode RespawnKey = KeyCode.R;

    public static string PlayerTag = "Player";

    // Movement 
    public const float WalkSpeed = 3f;
    public const float RunSpeed = 7f;
    public const float Gravity = -9.81f;
    public const float KillFloorHeight = -25;
    public static Vector3 PlayerResetPosition = new Vector3(210f, 380f, -247f);

    public const int MaxPlayerNameLength = 14;
    public const int MinPlayerNameLength = 3;

}
