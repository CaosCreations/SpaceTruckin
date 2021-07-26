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

    // Todo: Check if storing the Mission on the Bonus works better here
    [SerializeField] private Mission missionToApplyBonusTo;

    [SerializeField] private MissionBonus missionBonus;
    [SerializeField] private bool hasRandomBonus;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    public const string FOLDER_NAME = "MessageSaveData";

    [Serializable]
    public class MessageSaveData
    {
        public bool isUnlocked;
        public bool hasBeenRead;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MessageSaveData>(name, FOLDER_NAME);
    }
}
