using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeleteScriptableObjects: EditorWindow
{
    [MenuItem("Tools/CaosCreations/Delete Scriptable Objects")]
    private static void DeleteData()
    {
        GetWindow<DeleteScriptableObjects>("Delete Scriptable Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a scriptable object to delete:", EditorStyles.whiteLargeLabel);

    }

    // EditorUtils 
    public static void ShowWindow()
    {
        // Pass in the script instead as a param 
        EditorWindow.GetWindow<DeleteScriptableObjects>();
    }

}
