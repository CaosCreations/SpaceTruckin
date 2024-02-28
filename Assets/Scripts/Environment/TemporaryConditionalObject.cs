using Events;
using System.Linq;
using UnityEngine;

public class TemporaryConditionalObject : MonoBehaviour
{
    [Header("Simple conditions")]
    [SerializeField]
    private Condition[] conditions;

    [SerializeField]
    private Operator op;

    [Header("Compound conditions")]
    [SerializeField]
    private ConditionMetaGroup conditionMetaGroup;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(EndOfDayHandler);
        SingletonManager.EventService.Add<OnEveningStartEvent>(EveningStartHandler);
        SingletonManager.EventService.Add<OnMessageReadEvent>(MessageReadHandler);
        SingletonManager.EventService.Add<OnMissionCompletedEvent>(MissionCompletedHandler);
        SingletonManager.EventService.Add<OnConversationEndedEvent>(ConversationEndedHandler);
        SingletonManager.EventService.Add<OnDialogueVariableUpdatedEvent>(DialogueVariableUpdatedHandler);
        ConditionallySetActive();
    }

    private bool IsActive()
    {
        // Compound 
        if (conditionMetaGroup != null && conditionMetaGroup.ConditionGroups.Any())
        {
            return conditionMetaGroup.IsMet();
        }
        // Simple (to keep supporting the old ones) 
        else
        {
            return op == Operator.And ? conditions.AreAllMet() : conditions.AreAnyMet();
        }
    }

    public void ConditionallySetActive()
    {
        var isActive = IsActive();
        Debug.Log($"ConditionallySetActive - {name} to {isActive}");
        gameObject.SetActive(isActive);
    }

    private void EndOfDayHandler(OnEndOfDayEvent evt)
    {
        ConditionallySetActive();
    }

    private void EveningStartHandler()
    {
        ConditionallySetActive();
    }

    private void MessageReadHandler()
    {
        ConditionallySetActive();
    }

    private void MissionCompletedHandler()
    {
        ConditionallySetActive();
    }

    private void ConversationEndedHandler(OnConversationEndedEvent evt)
    {
        ConditionallySetActive();
    }

    private void DialogueVariableUpdatedHandler()
    {
        ConditionallySetActive();
    }
}