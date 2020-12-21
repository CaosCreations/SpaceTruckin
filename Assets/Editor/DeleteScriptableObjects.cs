using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DeleteScriptableObjects: EditorWindow
{
    [SerializeField] private Mission[] missionContainer;
    [SerializeField] private Pilot[] pilotsContainer;


    [MenuItem("Tools/CaosCreations/Delete Scriptable Objects")]
    private static void DeleteData()
    {
        GetWindow<DeleteScriptableObjects>("Delete Scriptable Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a scriptable object to delete:", EditorStyles.whiteLargeLabel);
        //GUILayout.Label("Missions", EditorStyles.label);
        //GUILayout.Label("Pilots", EditorStyles.label);

        if (GUILayout.Button("Delete mission data"))
        {
            foreach (Mission mission in missionContainer)
            {
                foreach (PropertyInfo property in mission.GetType().GetProperties())
                {
                    property.SetValue(mission, null);
                }
            }

        }
        else if (GUILayout.Button("Delete pilot data"))
        {

        }

    }

    // EditorUtils 
    public static void ShowWindow()
    {
        // Pass in the script instead as a param 
        EditorWindow.GetWindow<DeleteScriptableObjects>();
    }

}
