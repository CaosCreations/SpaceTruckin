﻿using UnityEngine;

public class UIConstants : MonoBehaviour
{
    // Layers 
    public const int UILayer = 5;
    public const int ShipPreviewLayer = 9;
    public const int RepairsMinigameLayer = 10;


    // Camera priorities 
    public const int RepairsMinigameCameraOnPriority = 1000;
    public const int RepairsMinigameCameraOffPriority = 1;
    public const int RepairsMinigameCameraDepth = 1;


    // Colour palette
    public static readonly Color32 SpringWood = new Color32(0xf2, 0xf0, 0xe5, 0xff);
    public static readonly Color32 Chatelle = new Color32(0xb8, 0xb5, 0xb9, 0xff);
    public static readonly Color32 Mamba = new Color32(0x86, 0x81, 0x88, 0xff);
    public static readonly Color32 GunPowder = new Color32(0x45, 0x44, 0x4f, 0xff);
    public static readonly Color32 Martinique = new Color32(0xf2, 0xf0, 0xe5, 0xff);
    public static readonly Color32 Shark = new Color32(0x21, 0x21, 0x23, 0xff);
    public static readonly Color32 Blackcurrant = new Color32(0x35, 0x2b, 0x42, 0xff);
    public static readonly Color32 MulledWine = new Color32(0x43, 0x43, 0x6a, 0xff);
    public static readonly Color32 MulledWineShade1 = new Color32(0x4b, 0x41, 0x58, 0xff);
    public static readonly Color32 Indigo = new Color32(0x4b, 0x80, 0xca, 0xff);
    public static readonly Color32 Viking = new Color32(0x68, 0xc2, 0xd3, 0xff);
    public static readonly Color32 AquaIsland = new Color32(0xa2, 0xdc, 0xc7, 0xff);
    public static readonly Color32 Primrose = new Color32(0xed, 0xe1, 0x9e, 0xff);
    public static readonly Color32 Whiskey = new Color32(0xd3, 0xa0, 0x68, 0xff);
    public static readonly Color32 Matrix = new Color32(0xb4, 0x52, 0x52, 0xff);
    public static readonly Color32 SaltBox = new Color32(0x6a, 0x53, 0x6e, 0xff);
    public static readonly Color32 SaltBoxShade1 = new Color32(0x64, 0x63, 0x65, 0xff);
    public static readonly Color32 SaltBoxShade2 = new Color32(0x5f, 0x55, 0x6a, 0xff);
    public static readonly Color32 IronStone = new Color32(0x80, 0x49, 0x3a, 0xff);
    public static readonly Color32 Muesli = new Color32(0xa7, 0x7b, 0x5b, 0xff);
    public static readonly Color32 Tacha = new Color32(0xc2, 0xd3, 0x68, 0xff);
    public static readonly Color32 ChelseaCucumber = new Color32(0x8a, 0xb0, 0x60, 0xff);
    public static readonly Color32 CuttySark = new Color32(0x56, 0x7b, 0x79, 0xff);
    public static readonly Color32 GreyAsparagus = new Color32(0x4e, 0x58, 0x4a, 0xff);
    public static readonly Color32 YellowMetal = new Color32(0x7b, 0x72, 0x43, 0xff);
    public static readonly Color32 DoubleSpanishWhite = new Color32(0xe5, 0xce, 0xb4, 0xff);
    public static readonly Color32 GreenSmoke = new Color32(0xb2, 0xb4, 0x7e, 0xff);
    public static readonly Color32 BeautyBush = new Color32(0xed, 0xc8, 0xc4, 0xff);
    public static readonly Color32 Viola = new Color32(0xcf, 0x8a, 0xcb, 0xff);


    // UI element colours 
    public static readonly Color32 InactiveTabButtonColour = SpringWood;


    // UI element text
    public const string NextCardText = "Next";
    public const string CloseCardCycleText = "Close";
    public const string EveningStartText = "Evening begins...";
    public const string MorningStartText = "Morning begins...";
    public const string EndOfCalendarText = "End of calendar...";
    public const string GameOverText = "Game over...";


    // UI element dimensions 
    public static readonly Vector3 ShipPreviewOffset = new Vector3(-1.5f, 0f, 0f);
    public static readonly Vector3 ShipPreviewRotationSpeed = new Vector3(0f, 0.15f, 0f);
    public const float ShipPreviewScaleFactor = 0.85f;

    public const float ClockTextWidth = 128f;
    public const float ClockTextHeight = 64f;
    public static readonly float ClockTextXPosition = Camera.main.pixelWidth - 128f;
    public static readonly float ClockTextYPosition = /*Camera.main.pixelHeight -*/ 0f;


    // Bed canvas 
    public const float TimeToSleep = 4f;
    public const float TimeToDock = 2f;


    // Regex
    public const char TemplateBoundaryLeftChar = '{';
    public const char TemplateBoundaryRightChar = '}';
    private const int numberOfBoundaryChars = 2;
    public static readonly string TemplateBoundaryLeft = new string(TemplateBoundaryLeftChar, numberOfBoundaryChars);
    public static readonly string TemplateBoundaryRight = new string(TemplateBoundaryRightChar, numberOfBoundaryChars);

    // e.g. {{Foo}} 
    public static readonly string GameStateTemplatePattern = $"({TemplateBoundaryLeft}[^}}]*){TemplateBoundaryRight}";
    public const string UnsignedIntegerPattern = @"^[0-9]*$";
    public const string AlphabeticalPattern = @"^[a-zA-Z ]+$";
    public const string AlphabeticalIncludingAccentsPattern = @"[-'0-9a-zA-ZÀ-ÿ ]";
    public const string ConsecutiveSpacesPattern = @"[ ]{2,}";

    // e.g. [var=Foo]
    public const string LuaVariablePattern = @"(\[var=)([A-zA-Z]*)]";

    // e.g. [Actor["Foo"].Bar]
    public const string LuaActorFieldPattern = @"(\[Actor)\[""([A-zA-Z]*)""]\.([A-zA-Z]*)]";

    // Templates
    public const string PlayerNameTemplate = "PLAYERNAME";
    public const string ShipNameTemplate = "SHIPNAME";


    // Settings 
    public const float DefaultVolumeSliderValue = 1f;


    // Character creation
    public const float PlayerNameSettingsDelayInSeconds = .5f;
}
