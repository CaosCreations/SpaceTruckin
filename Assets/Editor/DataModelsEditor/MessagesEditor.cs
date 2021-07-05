using System;
using UnityEditor;
using UnityEngine;

public class MessagesEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Messages/Unlock All")]
    private static void UnlockAll()
    {
        try
        {
            var messageContainer = EditorHelper.GetAsset<MessageContainer>();
            foreach (var message in messageContainer.messages)
            {
                message.IsUnlocked = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Messages/Read All")]
    private static void ReadAll()
    {
        try
        {
            var messageContainer = EditorHelper.GetAsset<MessageContainer>();
            foreach (var message in messageContainer.messages)
            {
                message.HasBeenRead = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var messageContainer = EditorHelper.GetAsset<MessageContainer>();
        foreach (var message in messageContainer.messages)
        {
            SaveDataEditor.NullifyFields(message.saveData);
        }
    }
}
