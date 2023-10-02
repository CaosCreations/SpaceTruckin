using System;
using UnityEditor;
using UnityEngine;

public class MissionsEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Missions/Accept All", false, 1)]
    public static void AcceptAll() => SetHasBeenAccepted(true);

    [MenuItem("Space Truckin/Missions/Unaccept All", false, 2)]
    public static void UnacceptAll() => SetHasBeenAccepted(false);

    private static void SetHasBeenAccepted(bool hasBeenAccepted)
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();

            foreach (var mission in missionContainer.Elements)
            {
                mission.HasBeenAccepted = hasBeenAccepted;
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log("All missions accepted is now " + hasBeenAccepted.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Missions/Unlock All", false, 13)]
    private static void UnlockAll() => SetHasBeenUnlocked(true);

    private static void SetHasBeenUnlocked(bool hasBeenUnlocked)
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

            foreach (var mission in missionContainer.Elements)
            {
                mission.HasBeenUnlocked = hasBeenUnlocked;
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log($"All missions {(hasBeenUnlocked ? "unlocked" : "locked")}");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Missions/Lock All", false, 14)]
    private static void LockAll() => SetHasBeenUnlocked(false);

    [MenuItem("Space Truckin/Missions/Complete All", false, 15)]
    private static void CompleteAll() => SetHasBeenCompleted(true);

    [MenuItem("Space Truckin/Missions/Incomplete All", false, 16)]
    private static void IncompleteAll() => SetHasBeenCompleted(false);

    private static void SetHasBeenCompleted(bool hasBeenCompleted)
    {
        try
        {
            if (hasBeenCompleted)
            {
                SetHasBeenUnlocked(true);
            }

            var missionContainer = EditorHelper.GetAsset<MissionContainer>();

            foreach (var mission in missionContainer.Elements)
            {
                mission.NumberOfCompletions = hasBeenCompleted ? Math.Max(mission.NumberOfCompletions, 1) : 0;
            }

            EditorUtility.SetDirty(missionContainer);

            Debug.Log($"All missions {(hasBeenCompleted ? "completed" : "not completed")}");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Missions/All In Progress Have 1 Day Remaining", false, 25)]
    private static void AllToOneDayRemaining()
    {
        try
        {
            var missionContainer = EditorHelper.GetAsset<MissionContainer>();
            
            foreach (var mission in missionContainer.Elements)
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

            foreach (var mission in missionContainer.Elements)
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

        foreach (var mission in missionContainer.Elements)
        {
            SaveDataEditor.NullifyFields(mission.saveData);
            EditorUtility.SetDirty(mission);
        }

        EditorUtility.SetDirty(missionContainer);
    }
}