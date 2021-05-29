using System;
using UnityEngine;
using UnityEngine.Events;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance { get; private set; }

    public ClockManager ClockManager;

    [SerializeField] private CalendarData calendarData;

    public static UnityAction OnEndOfDay;

    #region Property Accessors
    public TimeSpan DayStartTime { get => calendarData.DayStartTime; }
    public TimeSpan DayEndTime { get => calendarData.DayEndTime; }
    public int RealTimeDayDurationInSeconds { get => calendarData.RealTimeDayDurationInSeconds; }
    public int DaysInMonth { get => calendarData.DaysInMonth; }
    public int MonthsInYear { get => calendarData.MonthsInYear; }
    public int CurrentDay { get => calendarData.SaveData.CurrentDay; }
    public int CurrentMonth { get => calendarData.SaveData.CurrentMonth; }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    public static void EndDay()
    {
        SingletonManager.SaveAllData();

        Instance.ClockManager.SetupClockForNextDay();
        
        // Notify other objects that the day has ended
        OnEndOfDay?.Invoke();
    }
}
