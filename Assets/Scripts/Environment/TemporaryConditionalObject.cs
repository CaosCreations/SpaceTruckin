using Events;
using UnityEngine;

public class TemporaryConditionalObject : MonoBehaviour
{
    [SerializeField]
    private Condition[] conditions;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(EndOfDayHandler);
        SingletonManager.EventService.Add<OnEveningStartEvent>(EveningStartHandler);
        SingletonManager.EventService.Add<OnMessageReadEvent>(MessageReadHandler);
        SingletonManager.EventService.Add<OnMissionCompletedEvent>(MissionCompletedHandler);
        SingletonManager.EventService.Add<OnConversationEndedEvent>(ConversationEndedHandler);
        SingletonManager.EventService.Add<OnDialogueVariableUpdatedEvent>(DialogueVariableUpdatedHandler);
        SetActive();
    }

    private bool IsActive()
    {
        return conditions.AreAllMet();
    }

    private void SetActive()
    {
        gameObject.SetActive(IsActive());
    }

    private void EndOfDayHandler(OnEndOfDayEvent evt)
    {
        SetActive();
    }

    private void EveningStartHandler()
    {
        SetActive();
    }

    private void MessageReadHandler()
    {
        SetActive();
    }

    private void MissionCompletedHandler()
    {
        SetActive();
    }

    private void ConversationEndedHandler(OnConversationEndedEvent evt)
    {
        SetActive();
    }

    private void DialogueVariableUpdatedHandler()
    {
        SetActive();
    }
}