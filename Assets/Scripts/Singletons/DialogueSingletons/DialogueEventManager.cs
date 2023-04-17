using Events;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Assets.Scripts.Singletons.DialogueSingletons
{
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
                Debug.Log($"Dialogue cutscene finished event call back fired and conversation configured. Starting conversation with name '{finishedEvent.Cutscene.ConversationSettings.ConversationName}'");
                DialogueManager.StartConversation(finishedEvent.Cutscene.ConversationSettings.ConversationName);
            }
        }
    }
}
