using UnityEditor;
using UnityEngine;
using System.IO;
using System.Reflection;

public class SaveDataEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Delete AppData")]
    private static void DeleteAll() 
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("<b>No save data to delete</b>");
            return;
        }
        Directory.Delete(Application.persistentDataPath, recursive: true);
        Debug.Log($"Save data deleted at {Application.persistentDataPath}");
    }

    [MenuItem("Space Truckin/Delete Container Save Data")]
    private static void DeleteContainerFromTruckinMenu() => DeleteContainer();

    [MenuItem("Assets/Delete Container Save Data")] // Right click in Project tab
    private static void DeleteContainerFromAssetContextMenu() => DeleteContainer();

    [MenuItem("Space Truckin/Delete All Container Save Data")]
    private static void DeleteAllContainerSaveData() => DeleteAllContainers();

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

    public static void NullifyFields<T>(T saveData)
    {
        foreach (FieldInfo field in saveData.GetType().GetFields())
        {
            field.SetValue(saveData, null);
        }
    }

    private static void DeleteAllContainers()
    {
        LicencesEditor.DeleteSaveData();
        ShipsEditor.DeleteSaveData();
    }
}
