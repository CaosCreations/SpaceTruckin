using System;
using UnityEditor;
using UnityEngine;

public class ShipsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Ships/Max Out Resources")]
    private static void MaxOutResources() => MaxOutOrDepleteResources(isMaxedOut: true);

    [MenuItem("Space Truckin/Ships/Deplete All Resources")]
    private static void DepleteResources() => MaxOutOrDepleteResources(isMaxedOut: false);

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
