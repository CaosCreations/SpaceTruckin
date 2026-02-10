using PixelCrushers.DialogueSystem;
using UnityEngine;

[AddComponentMenu("Pixel Crushers/Dialogue System/Trigger/Intro Animation Dialogue System Trigger")]
public class IntroAnimationDialogueSystemTrigger : DialogueSystemTrigger
{
    [Header("Intro Animation")]
    [SerializeField] private ActorIntroAnimationRegistry animationRegistry;
    [SerializeField] private ActorIntroAnimationPlayer animationPlayer;

    protected override void DoConversationAction(Transform actor)
    {
        if (string.IsNullOrEmpty(conversation))
        {
            return;
        }

        var introAnimationData = GetIntroAnimationData();

        if (introAnimationData == null || animationPlayer == null)
        {
            base.DoConversationAction(actor);
            return;
        }

        PlayIntroThenStartConversation(introAnimationData, actor);
    }

    private ActorIntroAnimationData GetIntroAnimationData()
    {
        if (animationRegistry == null)
        {
            return null;
        }

        var conversationAsset = DialogueManager.MasterDatabase.GetConversation(conversation);
        if (conversationAsset == null)
        {
            return null;
        }

        var conversantActor = DialogueManager.MasterDatabase.GetActor(conversationAsset.ConversantID);
        if (conversantActor == null)
        {
            return null;
        }

        var introAnimKey = DialogueDatabaseManager.GetActorFieldAsString(
            conversantActor.Name, DialogueConstants.IntroAnimationKeyFieldName);

        if (string.IsNullOrEmpty(introAnimKey))
        {
            return null;
        }

        return animationRegistry.GetByKey(introAnimKey);
    }

    private void PlayIntroThenStartConversation(ActorIntroAnimationData data, Transform actor)
    {
        void OnFinished()
        {
            animationPlayer.OnAnimationFinished -= OnFinished;
            base.DoConversationAction(actor);
        }

        animationPlayer.OnAnimationFinished += OnFinished;
        animationPlayer.Play(data);
    }
}
