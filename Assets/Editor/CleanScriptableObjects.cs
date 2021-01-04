using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CleanScriptableObjects : EditorWindow
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
        CleanSaveData();
    }

    private static void CleanSaveData()
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
        else if (selected is ShipsContainer)
        {
            ShipsContainer shipsContainer = (ShipsContainer)selected;
            foreach (Ship ship in shipsContainer.ships)
            {
                foreach (FieldInfo field in ship.shipSaveData.GetType().GetFields())
                {
                    field.SetValue(ship.shipSaveData, null);
                }
                EditorUtility.SetDirty(ship);
            }
        }
        else if (selected is MessagesContainer) 
        {
            MessagesContainer messagesContainer = (MessagesContainer)selected;
            foreach (Message message in messagesContainer.messages)
            {
                foreach (FieldInfo field in message.messageSaveData.GetType().GetFields())
                {
                    field.SetValue(message.messageSaveData, null);
                }
                EditorUtility.SetDirty(message);
            }
        }
        else
        {
            Debug.LogError("Invalid object selected: " + selected);
            EditorUtils.DisplayErrorDialogue("")
        }
    }
}
