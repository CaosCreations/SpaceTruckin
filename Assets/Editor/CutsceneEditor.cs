using System;
using UnityEditor;
using UnityEngine;

public class CutsceneEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Cutscenes/Play Cutscene")]
    public static void PlayCutscene()
    {
        try
        {
            DialogueEditor.LiftUIAccess();
            TimelineManager.PlayCutscene("Lori Tut Part 1");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
