using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Schedule", menuName = "ScriptableObjects/Schedule", order = 1)]
public class Schedule : ScriptableObject
{
    // Int key is the day of the calendar month 
    public Dictionary<int, Job> schedule = new Dictionary<int, Job>();

    public int numberOfDays = 28;  

    public void LogSchedule()
    {
        foreach (KeyValuePair<int, Job> entry in schedule)
        {
            Debug.Log($"{entry.Value.title} scheduled for the {entry.Key}. of this month");
        }
    }
}
