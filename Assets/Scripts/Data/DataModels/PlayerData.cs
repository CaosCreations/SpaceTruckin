using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject, IDataModel
{
    [Header("Data to update IN GAME")]
    public PlayerSaveData saveData;

    public const string FolderName = "PlayerSaveData";

    [Serializable]
    public class PlayerSaveData
    {
        public string playerName;
        public string playerSpriteName;

        public long playerMoney;
        public long playerTotalMoneyAcquired; // Used to unlock missions

        public int playerLicencePoints;
        public int playerTotalLicencePointsAcquired; // Used to unlock licence tiers

        public int playerRepairTools; // Used to attempt the repairs minigame 

        public AnimatorSettings animatorSettings; // Set during character select
    }

    public string PlayerName
    {
        get => saveData.playerName; set => saveData.playerName = value;
    }

    public string SpriteName
    {
        get => saveData.playerSpriteName; set => saveData.playerSpriteName = value;
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

    public int PlayerTotalLicencePointsAcquired
    {
        get => saveData.playerTotalLicencePointsAcquired;
        set => saveData.playerTotalLicencePointsAcquired = value;
    }

    public int PlayerRepairTools
    {
        get => saveData.playerRepairTools; set => saveData.playerRepairTools = value;
    }

    public AnimatorSettings AnimatorSettings
    {
        get => saveData.animatorSettings; set => saveData.animatorSettings = value;
    }

    [Header("Starting values")]
    public long PlayerStartingMoney;
    public int PlayerStartingLicencePoints;
    public int PlayerStartingRepairTools;
    public Licence[] PlayerStartingLicences;

    public void SetStartingValues()
    {
        PlayerMoney = PlayerStartingMoney;
        PlayerTotalMoneyAcquired = PlayerMoney;
        PlayerLicencePoints = PlayerStartingLicencePoints;
        PlayerRepairTools = PlayerStartingRepairTools;
        Array.ForEach(PlayerStartingLicences, (l) =>
        {
            PlayerManager.Instance.AcquireLicence(l, true);
        });
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PlayerSaveData>(name, FolderName);
    }

    public void ResetData()
    {
        throw new NotImplementedException();
    }
}