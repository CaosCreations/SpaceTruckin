using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject, IDataModel
{
    private PlayerSaveData saveData;

    public static string FOLDER_NAME = "PlayerSaveData";
    public static string FILE_NAME = "PlayerSave";

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
        string fileName = DataModelsUtils.GetUniqueFileName(FILE_NAME, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, this);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(FILE_NAME, saveData.guid);
        await DataModelsUtils.LoadFileAsync<PlayerSaveData>(fileName, FOLDER_NAME);
    }
}