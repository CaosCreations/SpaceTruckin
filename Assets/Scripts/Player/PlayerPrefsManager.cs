using UnityEngine;

public static class PlayerPrefsManager
{
    #region UI
    // Viewed UI keys - used to check if the player has seen the UI before
    public static readonly string TerminalHasBeenViewedKey = "TerminalHasBeenViewed";
    public static readonly string HangarHasBeenViewedKey = "HangarHasBeenViewed";
    public static readonly string VendingHasBeenViewedKey = "VendingHasBeenViewed";
    public static readonly string CasetteHasBeenViewedKey = "CasetteHasBeenViewed";
    public static readonly string NoticeBoardHasBeenViewedKey = "NoticeBoardHasBeenViewed";
    public static readonly string BedHasBeenViewedKey = "BedHasBeenViewed";
    public static readonly string MainMenuHasBeenViewedKey = "MainMenuHasBeenViewed";

    public static bool GetHasBeenViewedPref(UICanvasType canvasType)
    {
        string key = GetHasBeenViewedKey(canvasType);
        return GetBool(key);
    }

    public static void SetHasBeenViewedPref(UICanvasType canvasType, bool value)
    {
        string key = GetHasBeenViewedKey(canvasType);
        SetBool(key, value);
    }

    private static string GetHasBeenViewedKey(UICanvasType canvasType)
    {
        string key;
        switch (canvasType)
        {
            case UICanvasType.Terminal:
                key = TerminalHasBeenViewedKey;
                break;
            case UICanvasType.Hangar:
                key = HangarHasBeenViewedKey;
                break;
            case UICanvasType.Vending:
                key = VendingHasBeenViewedKey;
                break;
            case UICanvasType.Cassette:
                key = CasetteHasBeenViewedKey;
                break;
            case UICanvasType.NoticeBoard:
                key = NoticeBoardHasBeenViewedKey;
                break;
            case UICanvasType.Bed:
                key = BedHasBeenViewedKey;
                break;
            case UICanvasType.MainMenu:
                key = MainMenuHasBeenViewedKey;
                break;
            default:
                Debug.LogError("Invalid UI type passed to PlayerPrefsManager.GetHasBeenViewedKey");
                return default;
        }
        return key;
    }

    #endregion

    private static bool GetBool(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue) != 0;
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value.ToInt());
    }

    private static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    private static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}