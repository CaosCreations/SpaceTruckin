using UnityEngine;

public static class PlayerPrefsManager
{
    public static bool GetCanvasTutorialPrefValue(UICanvasType canvasType, Date date)
    {
        string key = GetCanvasTutorialPrefKey(canvasType, date);
        return GetBool(key);
    }

    public static void SetCanvasTutorialPrefValue(UICanvasType canvasType, Date date, bool value)
    {
        string key = GetCanvasTutorialPrefKey(canvasType, date);
        SetBool(key, value);
    }

    private static string GetCanvasTutorialPrefKey(UICanvasType canvasType, Date date)
    {
        // e.g. "TUTORIAL_TERMINAL_1-1-1"
        return $"TUTORIAL_{canvasType.ToString().ToUpper()}_{date}";
    }

    private static bool GetBool(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue) != 0;
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value.ToInt());
    }
}