using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public class Message : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public MessageData data;

    [Header("Data to update IN GAME")]
    public MessageSaveData saveData;

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

    public void SaveData()
    {
        Debug.Log($"Saving mission: {data.messageName}_{saveData.guid}");

        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        if (!Directory.Exists(folderPath))
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        string filePath = Path.Combine(folderPath, $"{data.messageName}_{saveData.guid}.save");
        string fileContents = JsonUtility.ToJson(saveData);
        try
        {
            File.WriteAllText(filePath, fileContents);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        Debug.Log($"Finished saving mission: {data.messageName}_{saveData.guid}");

    }

    public void LoadData()
    {
        Debug.Log($"loading level: {data.messageName}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"{data.messageName}_{saveData.guid}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<MessageSaveData>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading level: {data.messageName}_{saveData.guid}");
    }

    public void DeleteData()
    {
        Debug.Log($"Deleting mission: {data.messageName}_{saveData.guid}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"/{data.messageName}_{saveData.guid}.save");
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}