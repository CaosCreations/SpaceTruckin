using System;
using UnityEditor;
using UnityEngine;

public class PlayerEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Player/Reset Money")]
    private static void ResetMoney()
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerMoney = 0;
            playerData.PlayerTotalMoneyAcquired = 0;
            Debug.Log("Player money reset");
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    [MenuItem("Space Truckin/Player/Give 1 $Million")]
    private static void GiveOneMillion()
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerMoney += 1000000;
            playerData.PlayerTotalMoneyAcquired += 1000000;
            Debug.Log("Player money = " + playerData.PlayerMoney.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    public static void SetMoney(long amount)
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            playerData.PlayerMoney = amount;
            Debug.Log("Player money = " + playerData.PlayerMoney.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError($"{ex.Message}\n{ex.StackTrace}");
        }
    }

    public static void DeleteSaveData()
    {
        var playerData = EditorHelper.GetAsset<PlayerData>();
        SaveDataEditor.NullifyFields(playerData.saveData);
    }
}
