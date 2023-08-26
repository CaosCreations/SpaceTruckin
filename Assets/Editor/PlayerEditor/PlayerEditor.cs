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

            if (EditorHelper.IsInPlayMode)
            {
                PlayerManager.Instance.ReceiveMoney(0);
            }

            Debug.Log("Player money reset");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Player/Give $1 Million")]
    private static void GiveOneMillion()
    {
        try
        {
            long moneyToGive = 1000000;

            var playerData = EditorHelper.GetAsset<PlayerData>();

            if (EditorHelper.IsInPlayMode)
            {
                PlayerManager.Instance.ReceiveMoney(moneyToGive);
            }
            else
            {
                playerData.PlayerMoney += moneyToGive;
                playerData.PlayerTotalMoneyAcquired += moneyToGive;
            }

            EditorUtility.SetDirty(playerData);

            Debug.Log("Player money = " + playerData.PlayerMoney.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void SetMoney(long amount)
    {
        try
        {
            var playerData = EditorHelper.GetAsset<PlayerData>();
            
            if (EditorHelper.IsInPlayMode)
            {
                playerData.PlayerMoney = 0;
                PlayerManager.Instance.ReceiveMoney(amount);
            }
            else
            {
                playerData.PlayerMoney = amount;
            }

            EditorUtility.SetDirty(playerData);

            Debug.Log("Player money = " + playerData.PlayerMoney.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Player/Toggle Baby Mode")]
    private static void ToggleBabyMode()
    {
        try
        {
            PlayerManager.PlayerMovementAnimation.ToggleBabyHolding();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var playerData = EditorHelper.GetAsset<PlayerData>();

        SaveDataEditor.NullifyFields(playerData.saveData);
        EditorUtility.SetDirty(playerData);
    }
}
