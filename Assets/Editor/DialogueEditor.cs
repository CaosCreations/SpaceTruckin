using System;
using UnityEditor;

public static class DialogueEditor
{
    [MenuItem("Space Truckin/Enable UI Access")]
    public static void EnableUIAccess()
    {
        SetUIAccess(true);
    }

    [MenuItem("Space Truckin/Disable UI Access")]
    public static void DisableUIAccess()
    {
        SetUIAccess(false);
    }

    private static void SetUIAccess(bool enabled)
    {
        var variableNames = new[] { "BedCanvasAccessible", "TerminalsAccessible" };
        Array.ForEach(variableNames, name =>
        {
            var val = DialogueDatabaseManager.GetLuaVariableAsBool(name);
            DialogueDatabaseManager.UpdateDatabaseVariable(name, enabled);
        });
    }
}
