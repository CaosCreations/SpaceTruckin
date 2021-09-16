using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;

public class ConversationTimelineCollisionTrigger : TimelineCollisionTrigger
{
    [SerializeField] private string conversationName;

    private void Awake()
    {
        // Restrict movement when timeline starts 
        playableDirector.played += PlayerManager.EnterPausedState;

        // Start conversation when timeline finishes
        playableDirector.stopped += StartConversation;
    }

    private void StartConversation(PlayableDirector playableDirector)
    {
        DialogueManager.StartConversation(conversationName);
    }
}
