using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotsConstants : MonoBehaviour
{
    // GameObject names 
    public static string profilePanelName = "PilotProfilePanel";
    public static string detailsObjectName = "PilotDetails";
    public static string pilotAvatarObjectName = "PilotAvatar";
    public static string shipAvatarObjectName = "ShipAvatar";
    public static string backButtonName = "BackButton";
    public static string backButtonText = "Back"; 


    // UI dimensions 
    public static float border = 0.05f;
    public static float topPadding = 32f;

    public static Vector2 pilotAvatarAnchorMin = new Vector2((1 - border * 2) / 1.5f, (1 - border * 2) / 1.5f);
    public static Vector2 pilotAvatarAnchorMax = new Vector2(1 - border, 1 - border);

    public static Vector2 pilotDetailsAnchorMin = new Vector2((1 - border * 2) / 1.5f, border);
    public static Vector2 pilotDetailsAnchorMax = new Vector2(1 - border, (1 - border * 2) / 1.5f);

    public static Vector2 shipAvatarAnchorMin = new Vector2(border, border);
    public static Vector2 shipAvatarAnchorMax = new Vector2((1 - border * 2) / 1.5f, 1 - border);

    public static Vector2 backButtonAnchorMin = new Vector2((1 - border * 2) / 2 - 0.2f, border);
    public static Vector2 backButtonAnchorMax = new Vector2((1 - border * 2) / 2 + 0.2f, border + 0.2f);


    // Paths to text files containing pilot names 
    public static string parentDirectoryPath = "./Assets/ImportedAssets/PilotNames/";
    public static string humanMaleNamesPath = parentDirectoryPath + "male_names.txt";
    public static string humanFemaleNamesPath = parentDirectoryPath + "female_names.txt";
    public static string helicidNamesPath = parentDirectoryPath + "helicid_names.txt";
    public static string oshunianNamesPath = parentDirectoryPath + "oshunian_first_names.txt";
    public static string oshunianTitlesPath = parentDirectoryPath + "oshunian_titles.txt";
    public static string vestaPrefixesPath = parentDirectoryPath + "vesta_prefixes.txt";
    public static string vestaNamesPath = parentDirectoryPath + "vesta_names.txt";

}
