using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobsManager : MonoBehaviour
{
    public Schedule schedule; 

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
        if (schedule.schedule.ContainsValue(job))
        {
            schedule.schedule.Remove(GetDayOfMonth(job)); 
        }

        schedule.schedule.Add(dayOfMonth, job);
        schedule.LogSchedule(); 
    }

    private void UnscheduleJob(Job job)
    {
        job.isScheduled = false; 
        int dayOfMonth = GetDayOfMonth(job);
        schedule.schedule.Remove(dayOfMonth); 
    }

    // We don't have access to the key in the DragNDrop and JobsUI scripts. 
    private int GetDayOfMonth(Job job)
    {
        // Get the corresponding key of the job in the dictionary. 
        int dayOfMonth = schedule.schedule.FirstOrDefault(k => k.Value == job).Key;
        return dayOfMonth; 
    }
}
