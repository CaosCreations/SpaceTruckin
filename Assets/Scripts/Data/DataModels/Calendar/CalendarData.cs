using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarData", menuName = "ScriptableObjects/CalendarData", order = 1)]
public class CalendarData : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public TimeSpan DayStartTime = new(6, 0, 0); // 6am
    public TimeSpan DayEndTime = new(26, 0, 0); // 2am the next day
    public int RealTimeDayDurationInSeconds = 900; // 15 mins 

    public int DaysInMonth = 28;
    public int MonthsInYear = 4;

    [field: SerializeField]
    public TimeOfDay StationEntryTimeOfDay { get; private set; }
        
    public TimeSpan EveningStartTime = new(18, 0, 0); // 6pm
    public TimeSpan MorningStartTime = new(6, 0, 0);

    public Date GameEndDate;

    public const string FolderName = "CalendarSaveData";
    public const string FileName = "CalendarData";

    public static string FilePath
    {
        get => DataUtils.GetSaveFilePath(FolderName, FileName);
    }

    [Header("Data to update IN GAME")]
    public Date CurrentDate;

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
