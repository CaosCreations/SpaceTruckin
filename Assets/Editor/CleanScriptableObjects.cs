using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CleanScriptableObjects: EditorWindow
{
    [MenuItem("Tools/Clean Scriptable Objects")]
    private static void CleanObjectsSelected()
    {
        CleanSelection();
    }

    private static void CleanSelection()
    {
        foreach (string guid in Selection.assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ScriptableObject)) as ScriptableObject;

            if (scriptableObject != null)
            {
                foreach (FieldInfo field in scriptableObject.GetType().GetFields())
                {
                    field.SetValue(scriptableObject, null);
                }
            }
        }
    }

    [MenuItem("Tools/Clean Scriptable Object Container Save Data")]
    private static void CleanContainerSaveData()
    {
        CleanPersistent();
    }

    private static void CleanPersistent()
    {
        var selected = Selection.activeObject;

        if (selected is MissionContainer)
        {
            MissionContainer missionContainer = (MissionContainer)selected;
            foreach (Mission mission in missionContainer.missions)
            {
                foreach (FieldInfo field in mission.missionSaveData.GetType().GetFields())
                {
                    field.SetValue(mission.missionSaveData, null);
                }
                EditorUtility.SetDirty(mission);
            }
        }
        else if (selected is PilotsContainer)
        {
            PilotsContainer pilotsContainer = (PilotsContainer)selected;
            foreach (Pilot pilot in pilotsContainer.pilots)
            {
                foreach (FieldInfo field in pilot.pilotSaveData.GetType().GetFields())
                {
                    field.SetValue(pilot.pilotSaveData, null);
                }
                EditorUtility.SetDirty(pilot);
            }
        }
        else
        {
            Debug.LogError("You have not selected a valid object");
        }
    }
}
