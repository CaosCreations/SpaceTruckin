using UnityEditor;

public static class DialogueEditor
{
    [MenuItem("Space Truckin/Dialogue/Lift UI Access")]
    public static void LiftUIAccess()
    {
        UIManager.LiftAccessSettings();
    }
}