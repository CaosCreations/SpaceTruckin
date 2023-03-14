using Events;
using PixelCrushers.DialogueSystem;
using System;
using UnityEngine;

public class CalendarManager : MonoBehaviour, IDataModelManager
{
    public static CalendarManager Instance { get; private set; }

    [SerializeField] private CalendarData calendarData;

    #region Property Accessors
    public static TimeSpan DayStartTime => Instance.calendarData.DayStartTime;
    public static TimeSpan DayEndTime => Instance.calendarData.DayEndTime;
    public static TimeSpan AwakeTimeDuration => DayEndTime.Subtract(DayStartTime);
    public static TimeSpan EveningStartTime => Instance.calendarData.EveningStartTime;
    public static TimeSpan MorningStartTime => Instance.calendarData.MorningStartTime;
    public static int RealTimeDayDurationInSeconds => Instance.calendarData.RealTimeDayDurationInSeconds;
    public static int DaysInMonth => Instance.calendarData.DaysInMonth;
    public static int DaysInYear => DaysInMonth * MonthsInYear;
    public static int MonthsInYear => Instance.calendarData.MonthsInYear;
    public static Date CurrentDate => Instance.calendarData.CurrentDate;
    public static int CurrentDay
    {
        get => Instance.calendarData.CurrentDate.Day;
        set => Instance.calendarData.CurrentDate.Day = value;
    }
    public static int CurrentMonth
    {
        get => Instance.calendarData.CurrentDate.Month;
        set => Instance.calendarData.CurrentDate.Month = value;
    }
    public static int CurrentYear
    {
        get => Instance.calendarData.CurrentDate.Year;
        set => Instance.calendarData.CurrentDate.Year = value;
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

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    public void Init()
    {
        if (calendarData == null)
        {
            Debug.LogError("No calendar data found");
            return;
        }
        calendarData.ValidateFields();
        RegisterLuaFunctions();
    }

    private void OnDisable()
    {
        UnregisterLuaFunctions();
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    public static void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        UpdateCalendarData();
        LogCalendarData();
    }

    private static void UpdateCalendarData()
    {
        // Wrap around when we reach the end of the month, and ensure we start at 1 instead of 0. 
        CurrentDay = CurrentDay
            .AddAndWrapAround(addend: 1, upperBound: DaysInMonth + 1, numberToWrapAroundTo: 1);

        if (CurrentDay <= 1)
        {
            // Wrap around when we reach the end of the year, starting at 1. 
            CurrentMonth = CurrentMonth
                .AddAndWrapAround(addend: 1, upperBound: MonthsInYear + 1, numberToWrapAroundTo: 1);

            if (CurrentMonth <= 1)
            {
                // Increment the year whenever we enter wrap around the month.
                CurrentYear++;
            }
        }
    }

    public static bool DateIsToday(Date date)
    {
        return date.Equals(CurrentDate);
    }

    private static void LogCalendarData()
    {
        Debug.Log("Current day: " + CurrentDay);
        Debug.Log("Current month: " + CurrentMonth);
        Debug.Log("Current year: " + CurrentYear);
    }

    #region Dialogue Integration
    // Parameters must be doubles as that's the numeric type Lua tables use.
    public bool HasDateBeenReached(double day, double month, double year = 1)
    {
        return CalendarUtils.ConvertDateToDays(CurrentDay, CurrentMonth, CurrentYear)
            >= CalendarUtils.ConvertDateToDays(day, month, year);
    }
    #endregion

    #region Lua Function Registration
    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.DateReachedFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => HasDateBeenReached(0, 0, 0)));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.DateReachedFunctionName);
    }
    #endregion

    #region Persistence
    public void SaveData()
    {
        calendarData.SaveData();
    }

    public async void LoadDataAsync()
    {
        if (!DataUtils.SaveFolderExists(CalendarData.FolderName))
        {
            DataUtils.CreateSaveFolder(CalendarData.FolderName);
            return;
        }

        await calendarData.LoadDataAsync();
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FolderName);
    }
    #endregion
}
