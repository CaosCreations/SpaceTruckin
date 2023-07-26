using Events;
using System.Linq;
using UnityEngine;

public class StationBlockingObjectSet : MonoBehaviour
{
    [SerializeField]
    private DateWithPhase[] activeDates;

    private void Start()
    {
        UpdateBlockingObjects();
        SingletonManager.EventService.Add<OnStationSetUpEvent>(OnStationSetUpHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
        SingletonManager.EventService.Add<OnEveningStartEvent>(OnEveningStartHandler);
    }

    private void UpdateBlockingObjects()
    {
        var currentDate = CalendarManager.CurrentDate;

        // If all active dates have passed, we no longer need this object set
        if (activeDates.All(d => d.Date > currentDate))
        {
            Debug.Log($"{nameof(StationBlockingObjectSet)} - All active dates passed for {gameObject.name}. Destroying set.");
            Destroy(gameObject);
            return;
        }

        var active = activeDates.Any(d => d.Date == CalendarManager.CurrentDate && (d.Phase == ClockManager.CurrentTimeOfDayPhase || d.Phase == TimeOfDay.Phase.Either));
        if (active)
        {
            Debug.Log($"{nameof(StationBlockingObjectSet)} - Setting {gameObject.name} children active.");
        }
        transform.SetDirectChildrenActive(active);
    }

    private void OnStationSetUpHandler(OnStationSetUpEvent evt)
    {
        UpdateBlockingObjects();
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        UpdateBlockingObjects();
    }

    private void OnEveningStartHandler()
    {
        UpdateBlockingObjects();
    }

    private void OnValidate()
    {
        for (int i = 0; i < activeDates.Length; i++)
        {
            activeDates[i].Date.Validate();
        }
    }
}
