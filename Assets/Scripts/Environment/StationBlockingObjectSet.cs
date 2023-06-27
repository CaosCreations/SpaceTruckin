using Events;
using System.Linq;
using UnityEngine;

public class StationBlockingObjectSet : MonoBehaviour
{
    [SerializeField]
    private Date[] activeDates;

    private void Start()
    {
        UpdateBlockingObjects();
        SingletonManager.EventService.Add<OnStationSetUpEvent>(OnStationSetUpHandler);
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    private void UpdateBlockingObjects()
    {
        var currentDate = CalendarManager.CurrentDate;

        // If all active dates have passed, we no longer need this object set
        if (!activeDates.Any(d => d <= currentDate))
        {
            Debug.Log($"{nameof(StationBlockingObjectSet)} - All active dates passed for {gameObject.name}. Destroying set.");
            Destroy(gameObject);
            return;
        }

        var active = activeDates.Contains(currentDate);
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

    private void OnValidate()
    {
        for (int i = 0; i < activeDates.Length; i++)
        {
            activeDates[i].Validate();
        }
    }
}
