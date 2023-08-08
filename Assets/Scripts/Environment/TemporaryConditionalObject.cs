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
        public ConditionType Type;
        public DateWithPhase[] ActiveDates;
        public Message Message;
        public Mission Mission;
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
        return conditions.All(c => c.Type switch
        {
            ConditionType.Date => c.ActiveDates.Any(d => d.Date == CalendarManager.CurrentDate && d.Phase == ClockManager.CurrentTimeOfDayPhase),
            ConditionType.Message => c.Message.HasBeenRead,
            ConditionType.Mission => c.Mission.HasBeenCompleted,
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