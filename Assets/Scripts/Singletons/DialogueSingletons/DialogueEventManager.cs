using Events;
using PixelCrushers.DialogueSystem;
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
        DialogueManager.Instance.conversationStarted += OnConversationStartedHandler;
        DialogueManager.Instance.conversationEnded += OnConversationEndedHandler;
        
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    private void OnConversationStartedHandler(Transform t)
    {
        Debug.Log("OnConversationStartedHandler: Last conversation started = " + DialogueManager.lastConversationStarted);
        var conversation = DialogueUtils.GetConversationByTitle(DialogueManager.lastConversationStarted);
        SingletonManager.EventService.Dispatch(new OnConversationStartedEvent(conversation));
    }

    private void OnConversationEndedHandler(Transform t)
    {
        Debug.Log("OnConversationEndedHandler: Last conversation ended = " + DialogueManager.lastConversationEnded);
        var conversation = DialogueUtils.GetConversationByTitle(DialogueManager.lastConversationStarted);
        SingletonManager.EventService.Dispatch(new OnConversationEndedEvent(conversation));
    }

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (evt.Cutscene.ConversationSettings != null && evt.Cutscene.ConversationSettings.StartConversationId > 0)
        {
            Debug.Log($"Dialogue cutscene finished event call back fired and conversation configured. Starting conversation with ID '{evt.Cutscene.ConversationSettings.StartConversationId}'...");
            DialogueUtils.StartConversationById(evt.Cutscene.ConversationSettings.StartConversationId);
        }
    }
}
