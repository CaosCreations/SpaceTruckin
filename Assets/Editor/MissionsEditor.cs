using System;
using UnityEditor;
using UnityEngine;

public class MissionsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Missions/Accept All")]
    private static void AcceptAll() => AcceptOrUnacceptAll(true);

    [MenuItem("Space Truckin/Missions/Unaccept All")]
    private static void UnacceptAll() => AcceptOrUnacceptAll(false);

    private static void AcceptOrUnacceptAll(bool hasBeenAccepted)
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            foreach (var mission in missionContainer.missions)
            {
                mission.HasBeenAccepted = hasBeenAccepted;
            }
            Debug.Log("All missions accepted is now " + hasBeenAccepted.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }
    [MenuItem("Space Truckin/Missions/Unlock All")]
    private static void UnlockAll()
    {
        try
        {
            long moneyNeeded = GetHighestMoneyNeededToUnlock();
            long currentMoney = EditorHelper.GetAsset<PlayerData>().PlayerMoney;

            if (currentMoney < moneyNeeded)
            {
                PlayerEditor.SetMoney(moneyNeeded);
            }
            Debug.Log("All missions unlocked");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    [MenuItem("Space Truckin/Missions/All In Progress Have 1 Day Remaining")]
    private static void AllToOneDayRemaining()
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            foreach (var mission in missionContainer.missions)
            {
                if (mission.IsInProgress())
                {
                    mission.DaysLeftToComplete = 1;
                }
            }
            Debug.Log("All in progress missions now have 1 day remaining");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    private static long GetHighestMoneyNeededToUnlock()
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            long highestValue = default;
            foreach (var mission in missionContainer.missions)
            {
                if (mission.MoneyNeededToUnlock > highestValue)
                {
                    highestValue = mission.MoneyNeededToUnlock;
                }
            }
            Debug.Log("Money needed to unlock all missions = " + highestValue.ToString());
            return highestValue;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
            return default;
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
