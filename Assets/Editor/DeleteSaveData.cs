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
            Debug.Log("No save data to delete");
            return;
        }
        Directory.Delete(Application.persistentDataPath, recursive: true);
        Debug.Log("Save data deleted");
    }

    [MenuItem("Space Truckin/Delete Scriptable Object Container Save Data")]
    private static void DeleteContainer()
    {
        var selected = Selection.activeObject;

        if (selected is MissionContainer)
        {
            MissionContainer missionContainer = (MissionContainer)selected;
            foreach (Mission mission in missionContainer.missions)
            {
                foreach (FieldInfo field in mission.saveData.GetType().GetFields())
                {
                    field.SetValue(mission.saveData, null);
                }
                EditorUtility.SetDirty(mission);
            }
        }
        else if (selected is PilotsContainer)
        {
            PilotsContainer pilotsContainer = (PilotsContainer)selected;
            foreach (Pilot pilot in pilotsContainer.pilots)
            {
                foreach (FieldInfo field in pilot.saveData.GetType().GetFields())
                {
                    field.SetValue(pilot.saveData, null);
                }
                EditorUtility.SetDirty(pilot);
            }
        }
        else if (selected is ShipsContainer)
        {
            ShipsContainer shipsContainer = (ShipsContainer)selected;
            foreach (Ship ship in shipsContainer.ships)
            {
                foreach (FieldInfo field in ship.saveData.GetType().GetFields())
                {
                    field.SetValue(ship.saveData, null);
                }
                EditorUtility.SetDirty(ship);
            }
        }
        else if (selected is MessageContainer)
        {
            MessageContainer messagesContainer = (MessageContainer)selected;
            foreach (Message message in messagesContainer.messages)
            {
                foreach (FieldInfo field in message.saveData.GetType().GetFields())
                {
                    field.SetValue(message.saveData, null);
                }
                EditorUtility.SetDirty(message);
            }
        }
        else
        {
            Debug.LogError("Invalid object selected: " + selected);

        }

    }
}
