using Events;
using System.Collections.Generic;
using UnityEditor;

public class ConversationSeenInfoEditorWindow : DataModelEditorWindow<ConversationSeenInfo>
{
    [MenuItem("Space Truckin/Dialogue/Conversation Seen Select")]
    public static void ShowWindow()
    {
        GetWindow<ConversationSeenInfoEditorWindow>("Conversation Seen Select");
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChangedHandler;
#endif
        if (SingletonManager.Instance != null)
        {
            SingletonManager.EventService.Add<OnCheckpointActivatedEvent>(OnCheckpointActivatedHandler);
            SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
            RefreshData(DialogueDatabaseManager.GetSeenInfo());
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeStateChangedHandler;
#endif
        if (SingletonManager.Instance != null)
        {
            SingletonManager.EventService.Remove<OnCheckpointActivatedEvent>(OnCheckpointActivatedHandler);
            SingletonManager.EventService.Remove<OnConversationEndedEvent>(OnConversationEndedHandler);
        }
        dataModels = null;
    }

    private void OnCheckpointActivatedHandler()
    {
        RefreshData(DialogueDatabaseManager.GetSeenInfo());
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        RefreshData(DialogueDatabaseManager.GetSeenInfo());
    }

    private void OnPlayModeStateChangedHandler(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            RefreshData(DialogueDatabaseManager.GetSeenInfo());
        }
    }

    protected override void HandleCustomToggleBehavior(IEditableDataModel model)
    {
        if (model is ConversationSeenInfo seenInfo)
        {
            seenInfo.SeenVariableKvp = new KeyValuePair<string, bool>(seenInfo.SeenVariableKvp.Key, seenInfo.ToggleValue);
            DialogueDatabaseManager.Instance.UpdateDatabaseVariable(seenInfo.SeenVariableKvp.Key, seenInfo.ToggleValue);
        }
    }
}
