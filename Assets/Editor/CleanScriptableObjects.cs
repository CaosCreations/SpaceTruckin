using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CleanScriptableObjects: EditorWindow
{
    [MenuItem("Tools/Clean Scriptable Objects")]
    private static void CleanData()
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

    [MenuItem("Tools/Clean Scriptable Object Save Data")]
    private static void CleanSaveData()
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
    }


    
    private static void RecursivelyCleanData(object data)
    {
        if (System.Array.TrueForAll(data.GetType().GetFields(), x => x == null))
        {
            return; 
        }
        foreach (FieldInfo field in data.GetType().GetFields())
        {
            field.SetValue(data, null);
            RecursivelyCleanData(field);
        }
    }
}
