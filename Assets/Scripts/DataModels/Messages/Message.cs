using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public partial class Message : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public string messageName, sender, subject, body;
    public int condition;

    // The mission offered in the email
    public Mission mission;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

    public static string FOLDER_NAME = "MessageSaveData";

    [Serializable]
    public class MessageSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool isUnlocked;
    }

    public void SaveData()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(messageName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(messageName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<MessageSaveData>(fileName, FOLDER_NAME);
    }
}