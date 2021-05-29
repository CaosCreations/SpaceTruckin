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

    [Header("Data to update IN GAME")]
    public CalendarSaveData saveData;

    public const string FOLDER_NAME = "CalendarSaveData";

    [Serializable]
    public class CalendarSaveData
    {
        public int CurrentDay;
        public int CurrentMonth;
        public int CurrentYear;
    }

    private void OnValidate()
    {
        DaysInMonth = Mathf.Max(DaysInMonth, 1);
        MonthsInYear = Mathf.Max(MonthsInYear, 1);

        saveData.CurrentDay = Mathf.Max(saveData.CurrentDay, 1);
        saveData.CurrentDay = Mathf.Max(saveData.CurrentDay, 1);
        saveData.CurrentMonth = Mathf.Max(saveData.CurrentMonth, 1);
        saveData.CurrentYear = Mathf.Max(saveData.CurrentYear, 1);
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
