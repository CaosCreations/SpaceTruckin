using UnityEngine;

public class DialogueCollisionTrigger : CollisionTriggerBehaviour
{
    [SerializeField] private int conversationId;

    protected override void TriggerBehaviour()
    {
        DialogueUtils.StartConversationById(conversationId);
    }
}
