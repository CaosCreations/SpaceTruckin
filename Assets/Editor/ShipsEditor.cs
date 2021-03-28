using UnityEditor;
using UnityEngine;

public class ShipsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Ships/Max Out Resources")]
    private static void MaxOutResources()
    {
        var shipsContainer = EditorHelper.GetAsset<ShipsContainer>();
        foreach (var ship in shipsContainer.ships)
        {
            ship.CurrentFuel = ship.MaxFuel;
            ship.CurrentHullIntegrity = ship.MaxHullIntegrity;
            ship.CanWarp = true;
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
