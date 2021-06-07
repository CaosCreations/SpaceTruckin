using System;
using UnityEngine;
using UnityEngine.Events;

public class CalendarManager : MonoBehaviour, IDataModelManager
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
    public int CurrentDay 
    { 
        get => calendarData.CurrentDate.Day;
        set => calendarData.CurrentDate.Day = value;
    }
    public int CurrentMonth 
    { 
        get => calendarData.CurrentDate.Month;
        set => calendarData.CurrentDate.Month = value;
    }
    public int CurrentYear
    {
        get => calendarData.CurrentDate.Year;
        set => calendarData.CurrentDate.Year = value;
    }
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

    public void Init()
    {
        if (DataUtils.SaveFolderExists(CalendarData.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(CalendarData.FOLDER_NAME);
        }

        if (calendarData == null)
        {
            Debug.LogError("No calendar data found");
        }
    }

    public static Date GetDateNow()
    {
        return new Date
        {
            Day = Instance.CurrentDay,
            Month = Instance.CurrentMonth,
            Year = Instance.CurrentYear
        };
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    public static void EndDay()
    {
        if (Instance.calendarData != null)
        {
            UpdateCalendarData();
            LogCalendarData();
        }

        // Notify other objects that the day has ended
        OnEndOfDay?.Invoke();

        Instance.ClockManager.SetupClockForNextDay();
    }

    private static void UpdateCalendarData()
    {
        // Wrap around when we reach the end of the month, and ensure we start at 1 instead of 0. 
        Instance.CurrentDay = Instance.CurrentDay
            .AddAndWrapAround(addend: 1, upperBound: Instance.DaysInMonth + 1, numberToWrapAroundTo: 1);

        if (Instance.CurrentDay <= 1)
        {
            // Wrap around when we reach the end of the year, starting at 1. 
            Instance.CurrentMonth = Instance.CurrentMonth
                .AddAndWrapAround(addend: 1, upperBound: Instance.MonthsInYear + 1, numberToWrapAroundTo: 1);

            if (Instance.CurrentMonth <= 1)
            {
                // Increment the year whenever we enter wrap around the month.
                Instance.CurrentYear++;
            }
        }
    }

    private static void LogCalendarData()
    {
        Debug.Log("Current day: " + Instance.CurrentDay);
        Debug.Log("Current month: " + Instance.CurrentMonth);
        Debug.Log("Current year: " + Instance.CurrentYear);
    }

    #region Persistence
    public void SaveData()
    {
        calendarData.SaveData();
    }

    public async void LoadDataAsync()
    {
        await calendarData.LoadDataAsync();
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FOLDER_NAME);
    }
    #endregion
}
