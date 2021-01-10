using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject, IDataModel
{
    public PlayerSaveData saveData;
    private const string FOLDER_NAME = "PlayerSaveData";
    private const string FILE_NAME = "PlayerSave";

    public class PlayerSaveData
    {
        public long playerMoney;
        public long playerTotalMoneyAcquired; //Used to unlock missions
        public Guid guid = new Guid();
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

    public void SaveData()
    {
        string fileName = $"{FILE_NAME}_{saveData.guid}";
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, this);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = $"{FILE_NAME}_{saveData.guid}";
        await DataModelsUtils.LoadFileAsync<PlayerSaveData>(fileName, FOLDER_NAME);
    }
}