using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobsManager : MonoBehaviour
{
    private void Start()
    {
        MessageDetailView.onJobAccept += AcceptJob;
        ScheduleSlot.onJobSchedule += ScheduleJob;
        DragNDrop.onUnscheduleJob += UnscheduleJob; 
    }

    private void AcceptJob(Job job)
    {
        job.isAccepted = true;
        Debug.Log(job.title + " has been accepted"); 
    }

    private void ScheduleJob(int dayOfMonth, Job job)
    {
        job.isScheduled = true;
        
        // In case the job is being "rescheduled."
        // We don't want to have two entries with the same value. 
        if (ScheduleData.schedule.ContainsValue(job))
        {
            ScheduleData.schedule.Remove(GetDayOfMonth(job)); 
        }

        ScheduleData.schedule.Add(dayOfMonth, job);
        ScheduleData.LogSchedule(); 
    }

    private void UnscheduleJob(Job job)
    {
        job.isScheduled = false; 
        int dayOfMonth = GetDayOfMonth(job);
        ScheduleData.schedule.Remove(dayOfMonth); 
    }

    // We don't have access to the key in the DragNDrop script.
    private int GetDayOfMonth(Job job)
    {
        // Get the corresponding key of the job in the dictionary. 
        int dayOfMonth = ScheduleData.schedule.FirstOrDefault(k => k.Value == job).Key;
        return dayOfMonth; 
    }
}
