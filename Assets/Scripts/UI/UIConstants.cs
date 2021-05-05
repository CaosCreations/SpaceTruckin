using UnityEngine;

public class UIConstants : MonoBehaviour
{
    // Layers 
    public static int UILayer = 5;
    public static int ShipPreviewLayer = 9;
    public static int RepairsMinigameLayer = 10;

    // Colour palette
    public static Color32 SpringWood = new Color32(0xf2, 0xf0, 0xe5, 0xff);
    public static Color32 Chatelle = new Color32(0xb8, 0xb5, 0xb9, 0xff);
    public static Color32 Mamba = new Color32(0x86, 0x81, 0x88, 0xff);
    public static Color32 GunPowder = new Color32(0x45, 0x44, 0x4f, 0xff);
    public static Color32 Martinique = new Color32(0xf2, 0xf0, 0xe5, 0xff);
    public static Color32 Shark = new Color32(0x21, 0x21, 0x23, 0xff);
    public static Color32 Blackcurrant = new Color32(0x35, 0x2b, 0x42, 0xff);
    public static Color32 MulledWine = new Color32(0x43, 0x43, 0x6a, 0xff);
    public static Color32 MulledWineShade1 = new Color32(0x4b, 0x41, 0x58, 0xff);
    public static Color32 Indigo = new Color32(0x4b, 0x80, 0xca, 0xff);
    public static Color32 Viking = new Color32(0x68, 0xc2, 0xd3, 0xff);
    public static Color32 AquaIsland = new Color32(0xa2, 0xdc, 0xc7, 0xff);
    public static Color32 Primrose = new Color32(0xed, 0xe1, 0x9e, 0xff);
    public static Color32 Whiskey = new Color32(0xd3, 0xa0, 0x68, 0xff);
    public static Color32 Matrix = new Color32(0xb4, 0x52, 0x52, 0xff);
    public static Color32 SaltBox = new Color32(0x6a, 0x53, 0x6e, 0xff);
    public static Color32 SaltBoxShade1 = new Color32(0x64, 0x63, 0x65, 0xff);
    public static Color32 SaltBoxShade2 = new Color32(0x5f, 0x55, 0x6a, 0xff);
    public static Color32 IronStone = new Color32(0x80, 0x49, 0x3a, 0xff);
    public static Color32 Muesli = new Color32(0xa7, 0x7b, 0x5b, 0xff);
    public static Color32 Tacha = new Color32(0xc2, 0xd3, 0x68, 0xff);
    public static Color32 ChelseaCucumber = new Color32(0x8a, 0xb0, 0x60, 0xff);
    public static Color32 CuttySark = new Color32(0x56, 0x7b, 0x79, 0xff);
    public static Color32 GreyAsparagus = new Color32(0x4e, 0x58, 0x4a, 0xff);
    public static Color32 YellowMetal = new Color32(0x7b, 0x72, 0x43, 0xff);
    public static Color32 DoubleSpanishWhite = new Color32(0xe5, 0xce, 0xb4, 0xff);
    public static Color32 GreenSmoke = new Color32(0xb2, 0xb4, 0x7e, 0xff);
    public static Color32 BeautyBush = new Color32(0xed, 0xc8, 0xc4, 0xff);
    public static Color32 Viola = new Color32(0xcf, 0x8a, 0xcb, 0xff);


    // UI element colours 
    public static Color32 InactiveTabButtonColour = SpringWood;


    // UI element text
    public static string CloseCardCycleText = "Close";


    // UI element dimensions 
    public static Vector3 ShipPreviewOffset = new Vector3(-3f, 0f, 0f);
    public static Vector3 ShipPreviewRotationSpeed = new Vector3(0f, 0.15f, 0f);
    public static float ShipPreviewScaleFactor = 0.85f;


    // Regex
    public const char TemplateBoundaryLeftChar = '{';
    public const char TemplateBoundaryRightChar = '}';
    private const int numberOfBoundaryChars = 2;
    public static string TemplateBoundaryLeft = new string(TemplateBoundaryLeftChar, numberOfBoundaryChars);
    public static string TemplateBoundaryRight = new string(TemplateBoundaryRightChar, numberOfBoundaryChars);

    public static string TemplatePattern = $"({TemplateBoundaryLeft}[^}}]*){TemplateBoundaryRight}";
    public const string UnsignedIntegerPattern = "^[0-9]*$";
    public const string AlphabeticalPattern = @"^[a-zA-Z ]+$";
    public const string AlphabeticalIncludingAccentsPattern = @"[-'0-9a-zA-ZÀ-ÿ ]";


    // Templates
    public const string PlayerNameTemplate = "PLAYERNAME";
    public const string ShipNameTemplate = "SHIPNAME";

}
