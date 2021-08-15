using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class MissionImporter : MonoBehaviour
{
    [MenuItem("Space Truckin/Missions/Import Missions From CSV")]
    private static void ImportScriptableObjectsFromCsv()
    {
        try
        {
            string csvPath = EditorUtility.OpenFilePanel("Open CSV file", "", "csv");

            if (csvPath.Length > 0)
            {
                string[] csvRows = File.ReadAllLines(csvPath);

                foreach (string row in csvRows)
                {
                    ImportMissionFromCsvRow(row);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void ImportMissionFromCsvRow(string csvRow)
    {
        try
        {
            string[] csvFields = csvRow.Split(',');

            Mission importedMission = ScriptableObject.CreateInstance<Mission>();

            PropertyInfo[] missionProperties = importedMission.GetType().GetProperties();

            PropertyInfo[] propertiesToImport = missionProperties
                .Where(x => EditorConstants.MissionImportPropertyOrder.Contains(x.Name))
                .ToArray();

            // Order that matches the CSV columns
            //Array.Sort(EditorConstants.MissionImportPropertyOrder, propertiesToImport);

            for (int i = 0; i < csvFields.Length; i++)
            {
                // Do type conversion
                object castedValue = EditorHelper.ConvertRuntimeType(csvFields[i], propertiesToImport[i].PropertyType);

                propertiesToImport[i].SetValue(importedMission, castedValue, null);
            }

            EditorUtility.SetDirty(importedMission);

            string assetName = $"{EditorConstants.ImportedMissionsPath}/{importedMission.Name.Trim()}_ImportedMission";

            if (EditorHelper.AssetWithNameExists(importedMission.Name, new[] { EditorConstants.ImportedMissionsPath }))
            {
                // Enforce unique names 
                assetName += GUID.Generate().ToString();
            }

            // Associate the new SO with an asset so it can persist 
            AssetDatabase.CreateAsset(importedMission, $"{assetName}.asset");
            AssetDatabase.SaveAssets();

            Selection.activeObject = importedMission;
        }
        catch
        {
            // Bubble up
            throw;
        }
    }
}
