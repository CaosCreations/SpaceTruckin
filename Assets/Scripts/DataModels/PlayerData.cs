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
        public Guid guid = Guid.NewGuid();
        public long playerMoney;
        public long playerTotalMoneyAcquired; //Used to unlock missions
    }

    public Guid Guid { get => saveData.guid; set => saveData.guid = value; } // might not need this

    public long PlayerMoney
    {
        get => saveData.playerMoney; set => saveData.playerMoney = value;
    }

    public long PlayerTotalMoneyAcquired
    {
        get => saveData.playerTotalMoneyAcquired; 
        set => saveData.playerTotalMoneyAcquired = value;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, this);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        await DataModelsUtils.LoadFileAsync<PlayerSaveData>(name, FOLDER_NAME);
    }

    public void SetDefaults()
    {
        saveData = new PlayerSaveData()
        {
            playerMoney = 0,
            playerTotalMoneyAcquired = 0,
            guid = new Guid()
        };
    }
}