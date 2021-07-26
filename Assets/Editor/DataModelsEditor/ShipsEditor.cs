using System;
using UnityEditor;
using UnityEngine;

public class ShipsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Ships/Max Out Resources")]
    private static void MaxOutResources() => MaxOutOrDepleteResources(isMaxedOut: true);

    [MenuItem("Space Truckin/Ships/Deplete All Resources")]
    private static void DepleteResources() => MaxOutOrDepleteResources(isMaxedOut: false);

    [MenuItem("Space Truckin/Ships/Reset Repair Tools")]
    private static void ResetTools() => SetRepairTools(0);
    
    private static void SetRepairTools(int numberOfTools)
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerRepairTools = numberOfTools;

            EditorUtility.SetDirty(playerData);

            Debug.Log("Repair tools set to " + playerData.PlayerRepairTools);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void MaxOutOrDepleteResources(bool isMaxedOut)
    {
        try
        {
            var shipsContainer = EditorHelper.GetAsset<ShipsContainer>();

            foreach (var ship in shipsContainer.Elements)
            {
                if (isMaxedOut)
                {
                    ship.CurrentFuel = ship.MaxFuel;
                    ship.CurrentHullIntegrity = ship.MaxHullIntegrity;
                    ship.CanWarp = true;
                }
                else
                {
                    ship.CurrentFuel = 0;
                    ship.CurrentHullIntegrity = 0;
                    ship.CanWarp = false;
                }
            }

            EditorUtility.SetDirty(shipsContainer);

            string substring = isMaxedOut ? "maxed out" : "depleted";
            Debug.Log("Ships resources are now " + substring);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var shipsContainer = EditorHelper.GetAsset<ShipsContainer>();
        
        foreach (var ship in shipsContainer.Elements)
        {
            SaveDataEditor.NullifyFields(ship.saveData);
        }

        EditorUtility.SetDirty(shipsContainer);
    }
}
