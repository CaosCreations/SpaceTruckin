﻿using Events;
using System.Linq;
using UnityEngine;

public class TemporaryConditionalObject : MonoBehaviour
{
    public enum Condition
    {
        Date, Message, Mission
    }

    [SerializeField]
    private Condition condition;

    [SerializeField]
    private DateWithPhase[] activeDates;

    [SerializeField]
    private Message message;

    [SerializeField]
    private Mission mission;

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
        return condition switch
        {
            Condition.Date => activeDates.Any(d => d.Date == CalendarManager.CurrentDate && d.Phase == ClockManager.CurrentTimeOfDayPhase),
            Condition.Message => message.HasBeenRead,
            Condition.Mission => mission.HasBeenCompleted,
            _ => false,
        };
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