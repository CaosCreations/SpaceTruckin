using System;
using UnityEditor;
using UnityEngine;

public class CalendarEditor : MonoBehaviour
{
    public static void SetDate(Date date)
    {
        try
        {
            var calendarData = EditorHelper.GetAsset<CalendarData>();
            calendarData.CurrentDate = date;
            EditorUtility.SetDirty(calendarData);

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
