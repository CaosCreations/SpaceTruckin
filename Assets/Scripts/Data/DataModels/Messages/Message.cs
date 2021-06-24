using System;
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

    // The mission offered in the email
    [SerializeField] private Mission mission;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    public static string FOLDER_NAME = "MessageSaveData";

    [Serializable]
    public class MessageSaveData
    {
        public bool isUnlocked;
        public bool isUnread;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<MessageSaveData>(name, FOLDER_NAME);
    }
}
