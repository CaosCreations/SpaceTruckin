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
        EditorUtility.SetDirty(calendarData);
    }

    [MenuItem("Space Truckin/Calendar/Go to Morning")]
    public static void ToMorning()
    {
        Debug.Log("Going to morning...");
        if (ClockManager.CurrentTimeOfDayPhase == TimeOfDay.Phase.Morning)
        {
            Debug.Log("Already morning");
            return;
        }

        // 17.966667 hrs
        ClockManager.SetCurrentTime(93480, true);
    }

    [MenuItem("Space Truckin/Calendar/Go to Evening")]
    public static void ToEvening()
    {
        Debug.Log("Going to evening...");
        if (ClockManager.CurrentTimeOfDayPhase == TimeOfDay.Phase.Evening)
        {
            Debug.Log("Already evening");
            return;
        }

        // 25.966667 hrs
        ClockManager.SetCurrentTime(64680, true);
    }
}
