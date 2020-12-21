using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomHierarchyContextMenu : EditorWindow
{
    private static Vector2 positionInHierarchy;
    private static GameObject objectSelected;
    private static bool customContextMenuOpen; 

    private void Awake()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnRightClickHierarchy;

    }

    private static void OnRightClickHierarchy(int instanceId, Rect selection)
    {
        if (Event.current != null && selection.Contains(Event.current.mousePosition) && Event.current.button == 1)
        {
            objectSelected = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (objectSelected)
            {
                positionInHierarchy = Event.current.mousePosition; // set once above?
                customContextMenuOpen = true;

                // Override the default context menu 
                Event.current.Use();
            }
        }
        if (customContextMenuOpen)
        {
            if (GUI.Button(new Rect(positionInHierarchy.x, positionInHierarchy.y, 150, 20f), "Delete"))
            {
                Debug.Log("Fired");
            }
        }
    }

}
