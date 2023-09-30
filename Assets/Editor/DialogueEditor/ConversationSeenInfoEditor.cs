using Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ConversationSeenInfoEditor : EditorWindow
{
    private Vector2 scrollPosition;
    private string searchBarFilter = string.Empty;
    private List<ConversationSeenInfo> seenInfo;

    [MenuItem("Space Truckin/Dialogue/Conversation Seen Select")]
    public static void ShowWindow()
    {
        GetWindow<ConversationSeenInfoEditor>("Conversation Seen Select");
    }

    private void OnEnable()
    {
        if (CanSubscribe())
        {
            SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
        }
        seenInfo = DialogueDatabaseManager.GetSeenInfo();
    }

    private void OnDisable()
    {
        if (CanSubscribe())
        {
            SingletonManager.EventService.Remove<OnConversationEndedEvent>(OnConversationEndedHandler);
        }
    }

    private void OnGUI()
    {
        if (seenInfo == null)
        {
            return;
        }
        EditorGUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.LabelField("Filter:", GUILayout.MaxWidth(50));
        searchBarFilter = EditorGUILayout.TextField(searchBarFilter);
        EditorGUILayout.EndHorizontal();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (!seenInfo.Any())
        {
            GUILayout.Label("No data available.");
            return;
        }

        foreach (var info in seenInfo.Where(x => x.Title.Contains(searchBarFilter, StringComparison.InvariantCultureIgnoreCase)))
        {
            EditorGUILayout.BeginHorizontal();

            var newToggleValue = EditorGUILayout.ToggleLeft(info.Title, info.SeenVariableKvp.Value);
            if (newToggleValue != info.SeenVariableKvp.Value)
            {
                info.SeenVariableKvp = new KeyValuePair<string, bool>(info.SeenVariableKvp.Key, newToggleValue);
                DialogueDatabaseManager.Instance.UpdateDatabaseVariable(info.SeenVariableKvp.Key, newToggleValue);
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private bool CanSubscribe()
    {
        return FindObjectOfType<SingletonManager>() != null;
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        seenInfo = DialogueDatabaseManager.GetSeenInfo();
        Repaint();
    }
}
