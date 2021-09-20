using UnityEngine;

public static class PlayerPrefsManager
{
    #region UI
    // Viewed UI keys - used to check if the player has seen the UI before
    private const string TerminalHasBeenViewedKey = "TerminalHasBeenViewed";
    private const string HangarHasBeenViewedKey = "HangarHasBeenViewed";
    private const string VendingHasBeenViewedKey = "VendingHasBeenViewed";
    private const string CasetteHasBeenViewedKey = "CasetteHasBeenViewed";
    private const string NoticeBoardHasBeenViewedKey = "NoticeBoardHasBeenViewed";
    private const string BedHasBeenViewedKey = "BedHasBeenViewed";
    private const string MainMenuHasBeenViewedKey = "MainMenuHasBeenViewed";
    private const string PauseMenuHasBeenViewedKey = "PauseMenuHasBeenViewed";

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
            case UICanvasType.PauseMenu:
                key = PauseMenuHasBeenViewedKey;
                break;
            default:
                Debug.LogError("Invalid UI type passed to PlayerPrefsManager.GetHasBeenViewedKey");
                return default;
        }
        return key;
    }
    #endregion

    #region Audio
    // Audio mixer group level keys
    private const string MasterVolumeKey = "MasterVolumeLevel";
    private const string MusicVolumeKey = "MasterVolumeLevel";
    private const string SoundEffectVolumeKey = "SoundEffectVolumeLevel";

    // Defaults levels
    private const float MasterVolumeDefault = 0.8f;
    private const float MusicVolumeDefault = 0.8f;
    private const float SoundEffectVolumeDefault = 0.4f;

    public static float GetMixerGroupVolumePref(MixerGroup mixerGroup)
    {
        string key = GetMixerGroupVolumeKey(mixerGroup);
        return GetFloat(key, GetMixerGroupDefaultVolume(mixerGroup));
    }

    public static void SetMixerGroupVolumePref(MixerGroup mixerGroup, float value)
    {
        string key = GetMixerGroupVolumeKey(mixerGroup);
        SetFloat(key, value);
    }

    private static string GetMixerGroupVolumeKey(MixerGroup mixerGroup)
    {
        string key;
        switch (mixerGroup)
        {
            case MixerGroup.Master:
                key = MasterVolumeKey;
                break;
            case MixerGroup.Music:
                key = MusicVolumeKey;
                break;
            case MixerGroup.SoundEffects:
                key = SoundEffectVolumeKey;
                break;
            default:
                Debug.LogError("Invalid Mixer group type passed to PlayerPrefsManager.GetMixerGroupVolumeKey");
                return default;
        }
        return key;
    }

    private static float GetMixerGroupDefaultVolume(MixerGroup mixerGroup)
    {
        switch (mixerGroup)
        {
            case MixerGroup.Master:
                return MasterVolumeDefault;
            case MixerGroup.Music:
                return MusicVolumeDefault;
            case MixerGroup.SoundEffects:
                return SoundEffectVolumeDefault;
            default:
                Debug.LogError("Invalid Mixer group type passed to PlayerPrefsManager.GetMixerGroupDefaultVolume");
                return default;
        }
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

    private static float GetFloat(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    private static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
}
