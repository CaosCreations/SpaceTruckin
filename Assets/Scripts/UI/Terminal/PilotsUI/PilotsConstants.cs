using System;
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
    public static float borderSize = 0.05f;
    public static float topPadding = 32f;
    public static float buttonHeight = 0.1f; 

    public static ValueTuple<Vector2, Vector2> pilotAvatarAnchors = (new Vector2((1 - borderSize * 2) / 1.5f, (1 - borderSize * 2) / 1.5f), new Vector2(1 - borderSize, 1 - borderSize));
    public static ValueTuple<Vector2, Vector2> pilotDetailsAnchors = (new Vector2((1 - borderSize * 2) / 1.5f, borderSize), new Vector2(1 - borderSize, (1 - borderSize * 2) / 1.5f));
    public static ValueTuple<Vector2, Vector2> shipAvatarAnchors = (new Vector2(borderSize, borderSize), new Vector2((1 - borderSize * 2) / 1.5f, 1 - borderSize));
    public static ValueTuple<Vector2, Vector2> backButtonAnchors = (new Vector2(borderSize, borderSize), new Vector2((1 - borderSize * 2) / 3, borderSize + buttonHeight));
    public static ValueTuple<Vector2, Vector2> hireButtonAnchors = (new Vector2((1 - borderSize * 2) / 3, borderSize), new Vector2((1 - borderSize * 2) / 1.5f - borderSize, borderSize + buttonHeight));


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
