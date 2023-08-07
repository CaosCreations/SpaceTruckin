using Events;
using System;
using System.Linq;
using UnityEngine;

public class TemporaryConditionalObject : MonoBehaviour
{
    public enum ConditionType
    {
        Date, Message, Mission
    }

    [Serializable]
    public class Condition
    {
        public ConditionType type;

        public DateWithPhase[] activeDates;
        public Message message;
        public Mission mission;
    }

    [SerializeField]
    private Condition[] conditions;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(EndOfDayHandler);
        SingletonManager.EventService.Add<OnEveningStartEvent>(EveningStartHandler);
        SingletonManager.EventService.Add<OnMessageReadEvent>(MessageReadHandler);
        SingletonManager.EventService.Add<OnMissionCompletedEvent>(MissionCompletedHandler);
        SetActive();
    }

    private bool IsActive()
    {
        return conditions.All(c => c.type switch
        {
            ConditionType.Date => c.activeDates.Any(d => d.Date == CalendarManager.CurrentDate && d.Phase == ClockManager.CurrentTimeOfDayPhase),
            ConditionType.Message => c.message.HasBeenRead,
            ConditionType.Mission => c.mission.HasBeenCompleted,
            _ => false
        });
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
}