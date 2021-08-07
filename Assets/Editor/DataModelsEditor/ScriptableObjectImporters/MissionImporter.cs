using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class MissionImporter : MonoBehaviour
{
    [MenuItem("Space Truckin/Missions/Import Missions From CSV")]
    private static void ImportMissionsFromCsv()
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

            Mission newMission = ScriptableObject.CreateInstance<Mission>();

            PropertyInfo[] missionProperties = newMission.GetType().GetProperties();

            for (int i = 0; i < csvFields.Length; i++)
            {
                missionProperties[i].SetValue(newMission, csvFields[i]);
            }

            EditorUtility.SetDirty(newMission);

            string assetName = $"{EditorConstants.ImportedMissionsPath}/{newMission.Name.Trim()}";

            if (EditorHelper.AssetWithNameExists(newMission.Name, new[] { EditorConstants.ImportedMissionsPath }))
            {
                // Enforce unique names 
                assetName += GUID.Generate().ToString();
            }

            // Associate the new SO with an asset so it can persist 
            AssetDatabase.CreateAsset(newMission, $"{assetName}.asset");
            AssetDatabase.SaveAssets();

            Selection.activeObject = newMission;
        }
        catch
        {
            throw;
        }
    }
}
