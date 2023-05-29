using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;

public class DialogueEventManager : MonoBehaviour
{
    public static DialogueEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent finishedEvent)
    {
        if (finishedEvent.Cutscene.ConversationSettings != null
            && finishedEvent.Cutscene.ConversationSettings.CloseDialogueUIOnStart)
        {
            Debug.Log($"Dialogue cutscene finished event call back fired and conversation configured. Starting conversation with ID '{finishedEvent.Cutscene.ConversationSettings.ConversationId}'...");

            var conversation = DialogueManager.DatabaseManager.loadedDatabases.First().GetConversation(finishedEvent.Cutscene.ConversationSettings.ConversationId)
                ?? throw new System.Exception($"Conversation not found with ID '{finishedEvent.Cutscene.ConversationSettings.ConversationId}'. Check the Cutscene ScriptableObject settings.");

            Debug.Log($"Starting conversation with Title '{conversation.Title}'...");
            DialogueManager.StartConversation(conversation.Title);
        }
    }
}
