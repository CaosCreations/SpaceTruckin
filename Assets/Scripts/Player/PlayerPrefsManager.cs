using UnityEngine;

public static class PlayerPrefsManager
{
    #region UI
    // Used UI keys - to check if the player has used the UI before
    public static readonly string TerminalIsUsedKey = "TerminalIsUsed";
    public static readonly string HangarIsUsedKey = "HangarIsUsed";
    public static readonly string VendingIsUsedKey = "VendingIsUsed";
    public static readonly string CasetteIsUsedKey = "CasetteIsUsed";
    public static readonly string NoticeBoardIsUsedKey = "NoticeBoardIsUsed";
    public static readonly string BedIsUsedKey = "BedIsUsed";

    public static bool GetUsedUIPref(UICanvasType canvasType)
    {
        string key = GetUsedUIKeyByType(canvasType);
        return GetBool(key);
    }

    public static void SetUsedUIPref(UICanvasType canvasType)
    {
        string key = GetUsedUIKeyByType(canvasType);
        SetBool(key, true);
    }

    private static string GetUsedUIKeyByType(UICanvasType canvasType)
    {
        string key;
        switch (canvasType)
        {
            case UICanvasType.Terminal:
                key = TerminalIsUsedKey;
                break;
            case UICanvasType.Hangar:
                key = HangarIsUsedKey;
                break;
            case UICanvasType.Vending:
                key = VendingIsUsedKey;
                break;
            case UICanvasType.Cassette:
                key = CasetteIsUsedKey;
                break;
            case UICanvasType.NoticeBoard:
                key = NoticeBoardIsUsedKey;
                break;
            case UICanvasType.Bed:
                key = BedIsUsedKey;
                break;
            default:
                Debug.LogError("Invalid UI type passed to SetUsedUIPref");
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