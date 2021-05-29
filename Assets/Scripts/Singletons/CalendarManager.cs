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
    public int CurrentDay { get => calendarData.saveData.CurrentDay; }
    public int CurrentMonth { get => calendarData.saveData.CurrentMonth; }
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

    /// <summary>
    /// The day either ends when the player chooses to sleep or the time elapses.
    /// </summary>
    public static void EndDay()
    {
        // Notify other objects that the day has ended
        OnEndOfDay?.Invoke();

        Instance.ClockManager.SetupClockForNextDay();
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
