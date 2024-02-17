using System;
using UnityEditor;
using UnityEngine;

public class CutsceneEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Cutscenes/Lori Tut Part 1")]
    public static void LoriTutPart1()
    {
        try
        {
            DialogueEditor.LiftUIAccess();
            DialogueEditor.LoriDay1PreTerminal();
            TimelineManager.PlayCutscene("Lori Tut Part 1");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    //[MenuItem("Space Truckin/Cutscenes/")]
    //public static void PlayCutscene()
    //{
    //    try
    //    {
    //        DialogueEditor.LiftUIAccess();
    //        DialogueEditor.LoriDay1PreTerminal();
    //        TimelineManager.PlayCutscene("Lori Tut Part 1");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogException(ex);
    //    }
    //}
}
