using UnityEngine;

public class MessageConstants : MonoBehaviour
{
    public static readonly Color UnreadColour = new(0.705f, 0.321f, 0.321f);
    public static readonly Color ReadColour = new(0.541f, 0.690f, 0.376f);

    public const string MissionAcceptButtonText = "Accept";
    public const string MissionAcceptedText = "Mission Accepted!";

    public const int EmailLoremCount = 16;
}
