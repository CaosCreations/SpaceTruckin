using System;
using UnityEngine;

public class PilotsConstants : MonoBehaviour
{
    #region GameObject Names
    public const string ProfilePanelName = "PilotProfilePanel";
    public const string DetailsObjectName = "PilotDetails";
    public const string PilotAvatarObjectName = "PilotAvatar";
    public const string ShipAvatarObjectName = "ShipAvatar";
    public const string BackButtonName = "BackButton";
    public const string BackButtonText = "Back";
    #endregion

    #region UI Dimensions 
    public const float BorderSize = 0.05f;
    public const float TopPadding = 32f;
    public const float ButtonHeight = 0.1f; 

    public static ValueTuple<Vector2, Vector2> PilotAvatarAnchors = (new Vector2((1 - BorderSize * 2) / 1.5f, (1 - BorderSize * 2) / 1.5f), new Vector2(1 - BorderSize, 1 - BorderSize));
    public static ValueTuple<Vector2, Vector2> PilotDetailsAnchors = (new Vector2((1 - BorderSize * 2) / 1.5f, BorderSize), new Vector2(1 - BorderSize, (1 - BorderSize * 2) / 1.5f));
    public static ValueTuple<Vector2, Vector2> ShipAvatarAnchors = (new Vector2(BorderSize, BorderSize), new Vector2((1 - BorderSize * 2) / 1.5f, 1 - BorderSize));
    public static ValueTuple<Vector2, Vector2> BackButtonAnchors = (new Vector2(BorderSize, BorderSize), new Vector2((1 - BorderSize * 2) / 3, BorderSize + ButtonHeight));
    public static ValueTuple<Vector2, Vector2> HireButtonAnchors = (new Vector2((1 - BorderSize * 2) / 3, BorderSize), new Vector2((1 - BorderSize * 2) / 1.5f - BorderSize, BorderSize + ButtonHeight));
    #endregion

    #region Asset Bundling
    // Asset file paths 
    public static string BundleLoadingPath = Application.streamingAssetsPath;
    public const string PilotTextAssetLoader = "PilotAssetsManager";
    public const string PilotTextBundleName = "pilottextbundle";

    // Text asset names
    public const string HumanMaleNamesKey = "MaleNames";
    public const string HumanFemaleNamesKey = "FemaleNames";
    public const string HelicidNamesKey = "HelicidNames";
    public const string OshunianNamesKey = "OshunianNames";
    public const string OshunianTitlesKey= "OshunianTitles";
    public const string VestaPrefixesKey = "VestaPrefixes";
    public const string VestaNamesKey = "VestaNames";
    public const string PilotLikesKey = "PilotLikes";
    public const string PilotDislikesKey = "PilotDislikes";
    #endregion

    #region Pilot Name Config
    // Robot names 
    public const int RobotPrefixLength = 3;
    public const int RobotSuffixLength = 4;
    #endregion
}
