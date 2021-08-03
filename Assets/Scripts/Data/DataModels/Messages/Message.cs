using System;
using System.Threading.Tasks;
using UnityEngine;

public enum MessageUnlockCondition
{
    TotalMoney, Date
}

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Messages/Message", order = 1)]
public partial class Message : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private string messageName, sender, subject, body;

    [SerializeField] private MessageUnlockCondition unlockCondition;
    [SerializeField] private long moneyNeededToUnlock;
    [SerializeField] private Date dateToUnlockOn;

    [Tooltip("The mission offered in the email")]
    [SerializeField] private Mission missionProposition;

    // The Mission Bonus and which Mission to bind it to 
    [Tooltip("The bonus offered in the email. It is not necessarily applied to the proposed Mission")]
    [SerializeField] private MissionBonus missionBonus;
    [SerializeField] private Mission missionToApplyBonusTo;
    [SerializeField] private bool hasRandomBonus;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    public const string FolderName = "MessageSaveData";

    [Serializable]
    public class MessageSaveData
    {
        public bool isUnlocked;
        public bool hasBeenRead;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MessageSaveData>(name, FolderName);
    }
}
