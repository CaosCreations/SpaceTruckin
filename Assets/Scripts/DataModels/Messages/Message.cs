using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "ScriptableObjects/Message", order = 1)]
public class Message : ScriptableObject
{
    public class MessageSaveData
    {
        public bool isUnlocked;
    }

    [Header("Set in Editor")]
    public string sender, subject, body;
    public int condition;

    // The mission offered in the email
    public Mission mission;

    [Header("Data to update IN GAME")]
    public MessageSaveData messageSaveData;
}