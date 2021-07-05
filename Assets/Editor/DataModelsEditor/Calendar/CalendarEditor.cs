using System;
using UnityEngine;

public class CalendarEditor : MonoBehaviour
{
    public static void SetDate(Date date)
    {
        try
        {
            var calendarData = EditorHelper.GetAsset<CalendarData>();
            calendarData.CurrentDate = date;

            Debug.Log("Current date set to: " + calendarData.CurrentDate.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public static void DeleteSaveData()
    {
        var calendarData = EditorHelper.GetAsset<CalendarData>();
        SaveDataEditor.DefaultStructFields(ref calendarData.CurrentDate);
    }
}
