using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarData", menuName = "ScriptableObjects/CalendarData", order = 1)]
public class CalendarData : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public TimeOfDay DayStartTimeOfDay = new(6, 0, 0);
    public TimeOfDay DayEndTimeOfDay = new(26, 0, 0);
    public TimeOfDay MorningStartTimeOfDay = new(6, 0, 0);
    public TimeOfDay EveningStartTimeOfDay = new(18, 0, 0);
    public TimeOfDay StationEntryTimeOfDay = new(18, 0, 0);

    public int RealTimeDayDurationInSeconds = 900; // 15 mins 
    public int DaysInMonth = 28;
    public int MonthsInYear = 4;

    public TimeSpan DayStartTime;
    public TimeSpan DayEndTime;
    public TimeSpan MorningStartTime;
    public TimeSpan EveningStartTime;

    public Date GameEndDate;
    public Date[] TimeFreezeDates;

    public const string FolderName = "CalendarSaveData";
    public const string FileName = "CalendarData";

    public static string FilePath
    {
        get => DataUtils.GetSaveFilePath(FolderName, FileName);
    }

    [Header("Data to update IN GAME")]
    public Date CurrentDate;

    public void Init()
    {
        DayStartTime = DayStartTimeOfDay.ToTimeSpan();
        DayEndTime = DayEndTimeOfDay.ToTimeSpan();
        MorningStartTime = MorningStartTimeOfDay.ToTimeSpan();
        EveningStartTime = EveningStartTimeOfDay.ToTimeSpan();
    }

    private void OnEnable()
    {
        ValidateFields();
    }

    private void OnValidate()
    {
        ValidateFields();
    }

    public void ValidateFields()
    {
        // Cannot be below 1 
        DaysInMonth = Mathf.Max(DaysInMonth, 1);
        MonthsInYear = Mathf.Max(MonthsInYear, 1);

        CurrentDate.Day = Mathf.Max(CurrentDate.Day, 1);
        CurrentDate.Month = Mathf.Max(CurrentDate.Month, 1);
        CurrentDate.Year = Mathf.Max(CurrentDate.Year, 1);

        GameEndDate.Day = Mathf.Max(GameEndDate.Day, 1);
        GameEndDate.Month = Mathf.Max(GameEndDate.Month, 1);
        GameEndDate.Year = Mathf.Max(GameEndDate.Year, 1);

        // Cannot be above upper bounds 
        CurrentDate.Day = Mathf.Min(CurrentDate.Day, DaysInMonth);
        CurrentDate.Month = Mathf.Min(CurrentDate.Month, MonthsInYear);
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, CurrentDate);
    }

    public async Task LoadDataAsync()
    {
        string json = await DataUtils.ReadFileAsync(FilePath);
        CurrentDate = JsonUtility.FromJson<Date>(json);
    }
}
