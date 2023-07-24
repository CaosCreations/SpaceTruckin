using Events;
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
    private Date[] activeDates;

    [SerializeField]
    private Message message;

    [SerializeField]
    private Mission mission;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(EndOfDayHandler);
        SingletonManager.EventService.Add<OnMessageReadEvent>(MessageReadHandler);
        SetActive();
    }

    private bool IsActive()
    {
        return condition switch
        {
            Condition.Date => activeDates.Any(d => CalendarManager.CurrentDate == d),
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

    private void MessageReadHandler()
    {
        SetActive();
    }
}
