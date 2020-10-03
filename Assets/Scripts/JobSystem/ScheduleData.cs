using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleData : MonoBehaviour
{
    // Int key is the day of the calendar month 
    public static Dictionary<int, Job> schedule = new Dictionary<int, Job>();

    public static int numberOfDays = 14;  

    public static void LogSchedule()
    {
        foreach (KeyValuePair<int, Job> entry in schedule)
        {
            Debug.Log($"{entry.Value.title} scheduled for the {entry.Key}. of this month");
        }
    }
}
