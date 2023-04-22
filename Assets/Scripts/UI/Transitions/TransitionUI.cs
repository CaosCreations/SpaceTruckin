using Events;
using System.Linq;
using UnityEngine;

public class TransitionUI : MonoBehaviour
{
    public enum TransitionType
    {
        TimeOfDay, GameOver, EndOfCalendar
    }

    [SerializeField]
    private TransitionCanvas[] transitionCanvases;

    private void Start()
    {
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    public void BeginTransition(TransitionType type, string textContent)
    {
        var transitionCanvas = transitionCanvases.FirstOrDefault(canvas => canvas.TransitionType == type);
        transitionCanvas.BeginTransition(textContent);
        PlayerManager.EnterPausedState();
    }

    private void OnEveningStartHandler()
    {
        BeginTransition(TransitionType.TimeOfDay, UIConstants.EveningStartText);
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        if (CalendarManager.CurrentDate >= CalendarManager.GameEndDate)
            return;

        BeginTransition(TransitionType.TimeOfDay, UIConstants.MorningStartText);
    }
}