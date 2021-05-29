using System;
using System.Threading.Tasks;
using UnityEngine;

public class CalendarData : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    public TimeSpan DayStartTime = new TimeSpan(6, 0, 0); // 6am
    public TimeSpan DayEndTime = new TimeSpan(26, 0, 0); // 2am the next day
    public int RealTimeDayDurationInSeconds = 900; // 15 mins 

    public int DaysInMonth = 28;
    public int MonthsInYear = 4;

    [Header("Data to update IN GAME")]
    public CalendarSaveData saveData;

    public const string FOLDER_NAME = "CalendarSaveData";

    [Serializable]
    public class CalendarSaveData
    {
        public int CurrentDay;
        public int CurrentMonth;
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<CalendarSaveData>(name, FOLDER_NAME);
    }
}
