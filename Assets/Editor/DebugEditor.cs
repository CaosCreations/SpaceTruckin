using UnityEngine;
using UnityEditor;

public static class DebugEditor
{
    [MenuItem("Debug/Print Global Position")]
    public static void PrintGlobalPosition()
    {
        if (Selection.activeGameObject != null)
        {
            Debug.Log(Selection.activeGameObject.name + " global position: " + Selection.activeGameObject.transform.position);
        }
    }
}