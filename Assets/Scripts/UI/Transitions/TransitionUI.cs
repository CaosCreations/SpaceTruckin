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

    [Header("Transition texts")]
    [SerializeField] private string morningTransitionText = "Morning begins...";
    [SerializeField] private string eveningTransitionText = "Evening begins...";

    public bool IsTransitioning => transitionCanvases.Any(c => c.IsTransitioning);

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
        BeginTransition(TransitionType.TimeOfDay, eveningTransitionText);
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        if (CalendarManager.IsEndOfCalendar)
            return;

        BeginTransition(TransitionType.TimeOfDay, morningTransitionText);
    }
}