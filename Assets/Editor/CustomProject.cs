using UnityEngine;

public class CustomProject
{
    [ContextMenu("Data/Clean Scriptable Objects")]
    private static void CleanCustom()
    {
        CleanCustomHandler();
    }

    private static void CleanCustomHandler()
    {
        var selected = Selection.activeObject;
        Debug.Log("Name: " + selected.name);
    }

    [MenuItem("Assets/Data/Clean")]
    private static void Clean() 
    {
        var selected = Selection.activeObject;
        Debug.Log("Name: " + selected.name);
        

    }

    private static void DisplayErrorDialogue(string title, string message)
    {
        EditorUtility.DisplayDialogue(title, message, "Yes", "No");

    }
}

