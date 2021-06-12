﻿using PixelCrushers.DialogueSystem;
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
    public TimeSpan DayStartTime => calendarData.DayStartTime;
    public TimeSpan DayEndTime => calendarData.DayEndTime;
    public int RealTimeDayDurationInSeconds => calendarData.RealTimeDayDurationInSeconds;
    public int DaysInMonth => calendarData.DaysInMonth;
    public int MonthsInYear => calendarData.MonthsInYear;
    public Date CurrentDate => calendarData.CurrentDate;
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

        RegisterLuaFunctions();
    }

    private void OnDisable()
    {
        UnregisterLuaFunctions();
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

    private static double ConvertDateToDays(double day, double month, double year)
    {
        // Subtract 1 as years and months start at 1, not 0. 
        double yearsInDays = (year - 1) * Instance.MonthsInYear * Instance.DaysInMonth;
        double monthsInDays = (month - 1) * Instance.DaysInMonth;

        return yearsInDays + monthsInDays + day;
    }

    private static void LogCalendarData()
    {
        Debug.Log("Current day: " + Instance.CurrentDay);
        Debug.Log("Current month: " + Instance.CurrentMonth);
        Debug.Log("Current year: " + Instance.CurrentYear);
    }

    #region Dialogue Integration
    // Parameters must be doubles as that's the numeric type Lua tables use.
    public bool HasDateBeenReached(double day, double month, double year = 1)
    {
        return ConvertDateToDays(CurrentDay, CurrentMonth, CurrentYear)
            >= ConvertDateToDays(day, month, year);
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
        await calendarData.LoadDataAsync();
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FOLDER_NAME);
    }
    #endregion
}
