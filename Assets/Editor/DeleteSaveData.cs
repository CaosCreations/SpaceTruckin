using UnityEditor;
using UnityEngine;
using System.IO;
using System.Reflection;

public class DeleteSaveData : MonoBehaviour
{
    [MenuItem("Space Truckin/Delete Save Directory Recursively")]
    private static void DeleteAll() 
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("<b>No save data to delete</b>");
            return;
        }
        Directory.Delete(Application.persistentDataPath, recursive: true);
        Debug.Log("Save data deleted");
    }

    [MenuItem("Space Truckin/Delete Scriptable Object Container Save Data")]
    private static void DeleteContainerFromTruckinMenu() => DeleteContainer();

    [MenuItem("Assets/Delete Scriptable Object Container Save Data")]
    private static void DeleteContainerFromAssetContextMenu() => DeleteContainer();

    private static void DeleteContainer()
    {
        var selected = Selection.activeObject;

        if (selected is MissionContainer)
        {
            MissionContainer missionContainer = (MissionContainer)selected;
            foreach (Mission mission in missionContainer.missions)
            {
                NullifyFields(mission.saveData);
                EditorUtility.SetDirty(mission);
            }
        }
        else if (selected is PilotsContainer)
        {
            PilotsContainer pilotsContainer = (PilotsContainer)selected;
            foreach (Pilot pilot in pilotsContainer.pilots)
            {
                NullifyFields(pilot.saveData);
                EditorUtility.SetDirty(pilot);
            }
        }
        else if (selected is ShipsContainer)
        {
            ShipsContainer shipsContainer = (ShipsContainer)selected;
            foreach (Ship ship in shipsContainer.ships)
            {
                NullifyFields(ship.saveData);
                EditorUtility.SetDirty(ship);
            }
        }
        else if (selected is MessageContainer)
        {
            MessageContainer messagesContainer = (MessageContainer)selected;
            foreach (Message message in messagesContainer.messages)
            {
                NullifyFields(message.saveData);
                EditorUtility.SetDirty(message);
            }
        }
        else
        {
            Debug.LogError("<b>Invalid object selected:</b> " + selected);
            return;
        }
        Debug.Log("Successfully deleted save data for: " + selected);
    }

    private static void NullifyFields<T>(T saveData)
    {
        foreach (FieldInfo field in saveData.GetType().GetFields())
        {
            field.SetValue(saveData, null);
        }
    }
}
