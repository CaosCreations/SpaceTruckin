using System;
using UnityEngine;

public class PilotsConstants : MonoBehaviour
{
    // GameObject names 
    public const string ProfilePanelName = "PilotProfilePanel";
    public const string DetailsObjectName = "PilotDetails";
    public const string PilotAvatarObjectName = "PilotAvatar";
    public const string ShipAvatarObjectName = "ShipAvatar";
    public const string BackButtonName = "BackButton";
    public const string BackButtonText = "Back"; 


    // UI dimensions 
    public const float BorderSize = 0.05f;
    public const float TopPadding = 32f;
    public const float ButtonHeight = 0.1f; 

    public static ValueTuple<Vector2, Vector2> PilotAvatarAnchors = (new Vector2((1 - BorderSize * 2) / 1.5f, (1 - BorderSize * 2) / 1.5f), new Vector2(1 - BorderSize, 1 - BorderSize));
    public static ValueTuple<Vector2, Vector2> PilotDetailsAnchors = (new Vector2((1 - BorderSize * 2) / 1.5f, BorderSize), new Vector2(1 - BorderSize, (1 - BorderSize * 2) / 1.5f));
    public static ValueTuple<Vector2, Vector2> ShipAvatarAnchors = (new Vector2(BorderSize, BorderSize), new Vector2((1 - BorderSize * 2) / 1.5f, 1 - BorderSize));
    public static ValueTuple<Vector2, Vector2> BackButtonAnchors = (new Vector2(BorderSize, BorderSize), new Vector2((1 - BorderSize * 2) / 3, BorderSize + ButtonHeight));
    public static ValueTuple<Vector2, Vector2> HireButtonAnchors = (new Vector2((1 - BorderSize * 2) / 3, BorderSize), new Vector2((1 - BorderSize * 2) / 1.5f - BorderSize, BorderSize + ButtonHeight));

    
    // Paths to text files
    private const string parentDirectoryPath = "./Assets/ImportedAssets/Resources/Text/Pilots/";
    private const string pilotNameSubDirectoryPath = parentDirectoryPath + "PilotNames/";

    public const string HumanMaleNamesPath = pilotNameSubDirectoryPath + "male_names.txt";
    public const string HumanFemaleNamesPath = pilotNameSubDirectoryPath + "female_names.txt";
    public const string HelicidNamesPath = pilotNameSubDirectoryPath + "helicid_names.txt";
    public const string OshunianNamesPath = pilotNameSubDirectoryPath + "oshunian_first_names.txt";
    public const string OshunianTitlesPath = pilotNameSubDirectoryPath + "oshunian_titles.txt";
    public const string VestaPrefixesPath = pilotNameSubDirectoryPath + "vesta_prefixes.txt";
    public const string VestaNamesPath = pilotNameSubDirectoryPath + "vesta_names.txt";

    public const string PilotLikesPath = parentDirectoryPath + "pilot_likes.txt";
    public const string PilotDislikesPath = parentDirectoryPath + "pilot_dislikes.txt";


    // Pilot names 
    public const int RobotPrefixLength = 3;
    public const int RobotSuffixLength = 4;

}
