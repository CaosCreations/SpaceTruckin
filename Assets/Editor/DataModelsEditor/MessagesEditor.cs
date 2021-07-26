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

            foreach (var message in messageContainer.Elements)
            {
                message.IsUnlocked = true;
            }

            EditorUtility.SetDirty(messageContainer);

            Debug.Log("All messages are now unlocked");
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

            foreach (var message in messageContainer.Elements)
            {
                message.HasBeenRead = true;
            }

            EditorUtility.SetDirty(messageContainer);

            Debug.Log("All messages are now read");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var messageContainer = EditorHelper.GetAsset<MessageContainer>();

        foreach (var message in messageContainer.Elements)
        {
            SaveDataEditor.NullifyFields(message.saveData);
        }

        EditorUtility.SetDirty(messageContainer);
    }
}
