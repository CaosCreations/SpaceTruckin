using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Import : MonoBehaviour
{
    private static string pathToMissions = "Assets/ScriptableObjects/Missions/ImportedMissions";

    [MenuItem("Space Truckin/Import Mission From CSV")]
    private static void ImportMission()
    {
        string path = EditorUtility.OpenFilePanel("Open CSV file", "", "csv");
        if (path.Length > 0)
        {
            string[] fileContent = File.ReadAllLines(path);

            foreach(string row in fileContent)
            {
                string[] fields = row.Split(',');

                Mission newMission = ScriptableObject.CreateInstance<Mission>();
                newMission.missionDurationInDays = Convert.ToInt32(fields[0]);
                newMission.missionName = fields[1];
                newMission.customer = fields[2];
                newMission.cargo = fields[3];
                newMission.description = fields[4];
                newMission.fuelCost = Convert.ToInt32(fields[5]);
                newMission.reward = Convert.ToInt32(fields[6]);
                newMission.moneyNeededToUnlock = Convert.ToInt32(fields[7]);

                EditorUtility.SetDirty(newMission);

                string assetName = $"{pathToMissions}/{fields[1].Trim()}";
                if (AssetDatabase.FindAssets(fields[1], new[] { pathToMissions }).Length > 0)
                {
                    assetName += GUID.Generate().ToString();
                } 

                AssetDatabase.CreateAsset(newMission, assetName + ".asset");
                AssetDatabase.SaveAssets();

                Selection.activeObject = newMission; 
            }
        }
    }
}
