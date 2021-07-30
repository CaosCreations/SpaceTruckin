using System;
using UnityEditor;
using UnityEngine;

public class PilotsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Pilots/Hire All")]
    private static void HireAll() => HireOrFireAll(true);

    [MenuItem("Space Truckin/Pilots/Fire All")]
    private static void FireAll() => HireOrFireAll(false);

    private static void HireOrFireAll(bool isHired)
    {
        try
        {
            var pilotsContainer = EditorHelper.GetAsset<PilotsContainer>();

            foreach (var pilot in pilotsContainer.Elements)
            {
                pilot.IsHired = isHired;
            }

            EditorUtility.SetDirty(pilotsContainer);

            Debug.Log("All pilots hired is " + isHired.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Pilots/Reset All XP and Level")]
    private static void ResetAllXpAndLevel()
    {
        try
        {
            var pilotsContainer = EditorHelper.GetAsset<PilotsContainer>();

            foreach (var pilot in pilotsContainer.Elements)
            {
                pilot.Level = 1;
                pilot.CurrentXp = 0;
                pilot.RequiredXp = 0;
            }

            EditorUtility.SetDirty(pilotsContainer);

            Debug.Log("All pilots xp and level reset");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var pilotsContainer = EditorHelper.GetAsset<PilotsContainer>();

        foreach (var pilot in pilotsContainer.Elements)
        {
            SaveDataEditor.NullifyFields(pilot.PilotData);
            EditorUtility.SetDirty(pilot);
        }

        EditorUtility.SetDirty(pilotsContainer);
    }
}
