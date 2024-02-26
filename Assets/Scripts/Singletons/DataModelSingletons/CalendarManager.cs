using Events;
using PixelCrushers.DialogueSystem;
using System;
using System.Linq;
using UnityEngine;

public class CalendarManager : MonoBehaviour, IDataModelManager, ILuaFunctionRegistrar
{
    public static CalendarManager Instance { get; private set; }

    [SerializeField] private CalendarData calendarData;
    [SerializeField] private Cutscene endOfCalendarCutscene;

    #region Property Accessors
    public static TimeOfDay StationEntryTimeOfDay => Instance.calendarData.StationEntryTimeOfDay;
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
    public static Date GameEndDate
    {
        get => Instance.calendarData.GameEndDate;
        set => Instance.calendarData.GameEndDate = value;
    }
    public static Date[] TimeFreezeDates => Instance.calendarData.TimeFreezeDates;
    public static bool IsTimeFrozenToday => TimeFreezeDates != null && TimeFreezeDates.Length > 0 && TimeFreezeDates.Any(d => d == CurrentDate);
    public static bool IsEndOfCalendar => CurrentDate >= GameEndDate;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        calendarData.Init();
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayClockEvent>(OnEndOfDayClockHandler);
        SingletonManager.EventService.Add<OnCutsceneFinishedEvent>(OnCutsceneFinishedHandler);
    }

    public void Init()
    {
        if (calendarData == null)
        {
            Debug.LogError("No calendar data found");
            return;
        }
        calendarData.ValidateFields();
    }

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    public static void OnEndOfDayClockHandler(OnEndOfDayClockEvent evt)
    {
        UpdateCalendarData();
        Debug.Log("New day: " + CurrentDate);
        SingletonManager.EventService.Dispatch(new OnEndOfDayEvent());
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

    private void OnCutsceneFinishedHandler(OnCutsceneFinishedEvent evt)
    {
        if (evt.Cutscene == endOfCalendarCutscene)
        {
            Debug.Log("Current date has reached the game end date in the CalendarData. Transitioning to end of game...");
            EndCalendar();
        }
    }

    public static void EndCalendar()
    {
        Debug.Log("Calendar ending... Current date: " + CurrentDate);
        var textContent = IsEndOfCalendar ? UIConstants.EndOfCalendarText : UIConstants.GameOverText;
        UIManager.BeginTransition(TransitionUI.TransitionType.EndOfCalendar, textContent);
    }

    public static void ResetCalendar()
    {
        Instance.calendarData.ResetDate();
    }

    public static void SetDate(Date date)
    {
        Instance.calendarData.CurrentDate = date;
        Debug.Log("Current date set to: " + Instance.calendarData.CurrentDate.ToString());
    }

    #region Dialogue Integration
    // Parameters must be doubles as that's the numeric type Lua tables use.
    public bool HasDateBeenReached(double day, double month, double year = 1)
    {
        return CalendarUtils.ConvertDateToDays(CurrentDay, CurrentMonth, CurrentYear)
            >= CalendarUtils.ConvertDateToDays(day, month, year);
    }

    public bool IsCurrentDate(double day, double month, double year = 1)
    {
        return new Date(day, month, year) == CurrentDate;
    }
    #endregion

    #region Lua Function Registration
    public void RegisterLuaFunctions()
    {
        Lua.RegisterFunction(
            DialogueConstants.DateReachedFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => HasDateBeenReached(0, 0, 0)));

        Lua.RegisterFunction(
            DialogueConstants.IsCurrentDateFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => IsCurrentDate(0, 0, 0)));

        Lua.RegisterFunction(
            DialogueConstants.EndCalendarFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => EndCalendar()));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.DateReachedFunctionName);
        Lua.UnregisterFunction(DialogueConstants.IsCurrentDateFunctionName);
        Lua.UnregisterFunction(DialogueConstants.EndCalendarFunctionName);
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

    public void ResetData()
    {
        throw new NotImplementedException();
    }
    #endregion
}
