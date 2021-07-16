using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CalendarData", menuName = "ScriptableObjects/CalendarData", order = 1)]
public class CalendarData : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public TimeSpan DayStartTime = new TimeSpan(6, 0, 0); // 6am
    public TimeSpan DayEndTime = new TimeSpan(26, 0, 0); // 2am the next day
    public int RealTimeDayDurationInSeconds = 900; // 15 mins 

    public int DaysInMonth = 28;
    public int MonthsInYear = 4;

    public const string FOLDER_NAME = "CalendarSaveData";
    public const string FILE_NAME = "CalendarData";

    public static string FILE_PATH
    {
        get => DataUtils.GetSaveFilePath(FOLDER_NAME, FILE_NAME);
    }

    [Header("Data to update IN GAME")]
    public Date CurrentDate;

    private void OnEnable()
    {
        // OnValidate is only called in editor.
        ValidateFields();
    }

    private void OnValidate()
    {
        ValidateFields();
    }

    private void ValidateFields()
    {
        // Cannot be below 1 
        DaysInMonth = Mathf.Max(DaysInMonth, 1);
        MonthsInYear = Mathf.Max(MonthsInYear, 1);

        CurrentDate.Day = Mathf.Max(CurrentDate.Day, 1);
        CurrentDate.Month = Mathf.Max(CurrentDate.Month, 1);
        CurrentDate.Year = Mathf.Max(CurrentDate.Year, 1);

        // Cannot be above upper bounds 
        CurrentDate.Day = Mathf.Min(CurrentDate.Day, DaysInMonth);
        CurrentDate.Month = Mathf.Min(CurrentDate.Month, MonthsInYear);
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, CurrentDate);
    }

    public async Task LoadDataAsync()
    {
        string json = await DataUtils.ReadFileAsync(FILE_PATH);
        CurrentDate = JsonUtility.FromJson<Date>(json);
    }
}
