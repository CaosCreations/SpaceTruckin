using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public class Message : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public MessageData data;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    public string FOLDER_NAME { get; private set; } = "MessageSaveData";

    public class MessageData
    {
        public string messageName, sender, subject, body;
        public int condition;

        // The mission offered in the email
        public Mission mission;
    }

    public class MessageSaveData
    {
        public Guid guid = new Guid();
        public bool isUnlocked;
    }


    public bool IsUnlocked 
    { 
        get { return saveData.isUnlocked; } set { saveData.isUnlocked = value; } 
    }

    public void SaveData()
    {
        string fileName = $"{data.messageName}_{saveData.guid}";
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = $"{data.messageName}_{saveData.guid}";
        saveData = await DataModelsUtils.LoadFileAsync<MessageSaveData>(fileName, FOLDER_NAME);
    }
}