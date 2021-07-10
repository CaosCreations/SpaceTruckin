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

            foreach (var mission in missionContainer.Missions)
            {
                mission.AcceptMission();
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log("All missions accepted is now " + hasBeenAccepted.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
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

            var missionContainer = EditorHelper.GetAsset<MissionContainer>();

            foreach (var mission in missionContainer.Missions)
            {
                mission.UnlockIfConditionMet();
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log("All missions unlocked");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Missions/All In Progress Have 1 Day Remaining")]
    private static void AllToOneDayRemaining()
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            
            foreach (var mission in missionContainer.Missions)
            {
                if (mission.IsInProgress())
                {
                    mission.DaysLeftToComplete = 1;
                }
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log("All in progress missions now have 1 day remaining");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static long GetHighestMoneyNeededToUnlock()
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            long highestValue = default;

            foreach (var mission in missionContainer.Missions)
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
            Debug.LogException(ex);
            return default;
        }
    }

    public static void DeleteSaveData()
    {
        var missionContainer = EditorHelper.GetAsset<MissionContainer>();

        foreach (var mission in missionContainer.Missions)
        {
            SaveDataEditor.NullifyFields(mission.saveData);
        }

        EditorUtility.SetDirty(missionContainer);
    }
}
