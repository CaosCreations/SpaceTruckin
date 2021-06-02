using UnityEditor;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;

public class SaveDataEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Delete AppData")]
    private static void DeleteAppData() 
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("<b>No save data to delete</b>");
            return;
        }

        Directory.Delete(Application.persistentDataPath, recursive: true);
        Debug.Log($"Save data deleted at {Application.persistentDataPath}");
    }

    [MenuItem("Space Truckin/Delete PlayerPrefs")]
    private static void DeletePlayerPrefs()
    {
        try
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs deleted");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Delete Container Save Data")]
    private static void DeleteContainerFromTruckinMenu() => DeleteContainer();

    [MenuItem("Assets/Delete Container Save Data")] // Right click in Project tab
    private static void DeleteContainerFromAssetContextMenu() => DeleteContainer();

    [MenuItem("Space Truckin/Delete All Container Save Data")]
    private static void DeleteAllContainerSaveData() => DeleteAllContainers();

    private static void DeleteContainer()
    {
        try
        {
            var selected = Selection.activeObject;

            if (selected is MissionContainer missionContainer)
            {
                foreach (Mission mission in missionContainer.missions)
                {
                    NullifyFields(mission.saveData);
                    EditorUtility.SetDirty(mission);
                }
            }
            else if (selected is PilotsContainer pilotsContainer)
            {
                foreach (Pilot pilot in pilotsContainer.pilots)
                {
                    NullifyFields(pilot.saveData);
                    EditorUtility.SetDirty(pilot);
                }
            }
            else if (selected is ShipsContainer shipsContainer)
            {
                foreach (Ship ship in shipsContainer.ships)
                {
                    NullifyFields(ship.saveData);
                    EditorUtility.SetDirty(ship);
                }
            }
            else if (selected is MessageContainer messagesContainer)
            {
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
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
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
        try
        {
            MissionsEditor.DeleteSaveData();
            PilotsEditor.DeleteSaveData();
            ShipsEditor.DeleteSaveData();
            LicencesEditor.DeleteSaveData();
            PlayerEditor.DeleteSaveData();
            CalendarEditor.DeleteSaveData();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
