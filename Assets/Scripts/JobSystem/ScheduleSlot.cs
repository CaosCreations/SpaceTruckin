using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScheduleSlot : MonoBehaviour, IDropHandler
{
    public static event Action<int, Job> onJobSchedule;
    public int dayOfMonth; 

    public void OnDrop(PointerEventData eventData)
    {
        // DragNDrop has a reference to the schedule and job being scheduled 
        // But we probably shouldn't use DragNDrop to access everything... 
        DragNDrop dragNDrop = eventData.pointerDrag.GetComponent<DragNDrop>();
        Schedule schedule = dragNDrop.jobsPanel.GetComponent<JobsUI>().schedule;

        if (eventData.pointerDrag != null)
        {
            // Prevent double booking 
            if (schedule.schedule.ContainsKey(dayOfMonth))
            {
                return; 
            }

            Job scheduledJob = dragNDrop.job;

            // Store location of job to display in UI on reload 
            scheduledJob.scheduleSlotTransform = gameObject.transform; 

            // Parent job to this schedule slot 
            eventData.pointerDrag.transform.parent = scheduledJob.scheduleSlotTransform;

            onJobSchedule?.Invoke(dayOfMonth, scheduledJob);
        }
    }
}
