using System;
using UnityEditor;
using UnityEngine;

public class ShipsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Ships/Max Out Resources")]
    private static void MaxOutResources() => MaxOutOrDepleteResources(isMaxedOut: true);

    [MenuItem("Space Truckin/Ships/Deplete All Resources")]
    private static void DepleteResources() => MaxOutOrDepleteResources(isMaxedOut: false);

    [MenuItem("Space Truckin/Ships/Give 10 Repair Tools")]
    private static void Give10Tools() => GiveRepairTools(10);
    
    private static void GiveRepairTools(int numberOfTools)
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerRepairTools += numberOfTools;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    private static void MaxOutOrDepleteResources(bool isMaxedOut)
    {
        try
        {
            var shipsContainer = EditorHelper.GetAsset<ShipsContainer>();
            foreach (var ship in shipsContainer.ships)
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
            string substring = isMaxedOut ? "maxed out" : "depleted";
            Debug.Log("Ships resources are now " + substring);
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    public static void DeleteSaveData()
    {
        var shipsContainer = EditorHelper.GetAsset<ShipsContainer>();
        foreach (var ship in shipsContainer.ships)
        {
            SaveDataEditor.NullifyFields(ship.saveData);
        }
    }
}
