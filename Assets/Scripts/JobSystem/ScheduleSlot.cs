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
        if (eventData.pointerDrag != null)
        {
            // Prevent double booking 
            if (ScheduleData.schedule.ContainsKey(dayOfMonth))
            {
                return; 
            }

            Job scheduledJob = eventData.pointerDrag.GetComponent<DragNDrop>().job; 

            // Store location of job to display in UI on reload 
            scheduledJob.scheduleSlotTransform = gameObject.transform;

            // Parent job to this schedule slot 
            eventData.pointerDrag.transform.parent = scheduledJob.scheduleSlotTransform;

            onJobSchedule?.Invoke(dayOfMonth, scheduledJob);
        }
    }
}
