using System;
using UnityEditor;
using UnityEngine;

public class TutorialEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Tutorial/Enable Mission Terminal Tutorial")]
    public static void EnableMissionTerminalTutorial()
    {
        try
        {
            EnableTutorial(UICanvasType.Terminal, new Date(1, 1, 1));
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Tutorial/Enable General Terminal Tutorial")]
    public static void EnableGeneralTerminalTutorial()
    {
        try
        {
            EnableTutorial(UICanvasType.Terminal, new Date(2, 1, 1));
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Tutorial/Enable Hangar Node Terminal Tutorial")]
    public static void EnableHangarNodeTerminalTutorial()
    {
        try
        {
            EnableTutorial(UICanvasType.Hangar, new Date(2, 1, 1));
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void EnableTutorial(UICanvasType canvasType, Date date)
    {
        DialogueEditor.LiftUIAccess();
        SetAllTutorialPlayerPrefs(true);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(canvasType, date, false);
        CalendarEditor.SetDate(date);
    }

    private static void SetAllTutorialPlayerPrefs(bool value)
    {
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, new Date(1, 1, 1), value);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Terminal, new Date(2, 1, 1), value);
        PlayerPrefsManager.SetCanvasTutorialPrefValue(UICanvasType.Hangar, new Date(2, 1, 1), value);
    }
}
