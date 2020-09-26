using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobsManager : MonoBehaviour
{ 
    void Start()
    {
        MessageDetailView.onJobAccept += AcceptJob;
        ScheduleSlot.onJobSchedule += ScheduleJob;
    }

    public void AcceptJob(Job job)
    {
        job.isAccepted = true;
        Debug.Log(job.title + " is now accepted");
    }

    public void ScheduleJob(Job job)
    {
        job.isScheduled = true;
        Debug.Log(job.title + " is now scheduled");
    }
}
