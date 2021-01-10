using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public class Message : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public MessageData data;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    private const string FOLDER_NAME = "MessageSaveData";

    public class MessageData
    {
        public string messageName, sender, subject, body;
        public int condition;

        // The mission offered in the email
        public Mission mission;
    }

    public class MessageSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool isUnlocked;
    }


    public string Name
    {
        get => data.messageName; set => data.messageName = value;
    }

    public string Sender { get => data.sender; set => data.sender = value; }

    public string Subject { get => data.subject; set => data.subject = value; }

    public string Body { get => data.body; set => data.body = value; }

    public bool IsUnlocked 
    { 
        get => saveData.isUnlocked; set => saveData.isUnlocked = value;  
    }

    public int Condition { get => data.condition; set => data.condition = value; }

    public Mission Mission { get => data.mission; set => data.mission = value; }

    public void SaveData()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(data.messageName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(data.messageName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<MessageSaveData>(fileName, FOLDER_NAME);
    }
}