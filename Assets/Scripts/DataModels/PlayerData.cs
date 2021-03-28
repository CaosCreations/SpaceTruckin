using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject, IDataModel
{
    [Header("Data to update IN GAME")]
    public PlayerSaveData saveData;

    public static string FOLDER_NAME = "PlayerSaveData";

    [Serializable]
    public class PlayerSaveData
    {
        public long playerMoney;
        public long playerTotalMoneyAcquired; //Used to unlock missions
        
        public int playerLicencePoints;
        public int playerTotalLicencePointsAcquired; //Used to unlock licence tiers
    }

    public long PlayerMoney
    {
        get => saveData.playerMoney; set => saveData.playerMoney = value;
    }

    public long PlayerTotalMoneyAcquired
    {
        get => saveData.playerTotalMoneyAcquired; 
        set => saveData.playerTotalMoneyAcquired = value;
    }

    public int PlayerLicencePoints
    {
        get => saveData.playerLicencePoints; set => saveData.playerLicencePoints = value;

    }

    public int PlayerTotalLicencePointsSpent
    {
        get => saveData.playerTotalLicencePointsAcquired;
        set => saveData.playerTotalLicencePointsAcquired = value;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData); 
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PlayerSaveData>(name, FOLDER_NAME);
    }
}