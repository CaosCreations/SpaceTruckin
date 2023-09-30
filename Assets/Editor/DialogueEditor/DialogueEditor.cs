using UnityEditor;
using UnityEngine;

public static class DialogueEditor
{
    [MenuItem("Space Truckin/Dialogue/Lift UI Access")]
    public static void LiftUIAccess()
    {
        UIManager.LiftAccessSettings();
    }

    [MenuItem("Space Truckin/Dialogue/Print Seen Vars")]
    public static void PrintSeenVars()
    {
        if (DialogueDatabaseManager.Instance == null)
        {
            return;
        }
        var info = DialogueDatabaseManager.GetSeenInfo();
        info.ForEach(i => Debug.Log(i));
    }
}
