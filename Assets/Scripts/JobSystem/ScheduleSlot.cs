using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScheduleSlot : MonoBehaviour, IDropHandler
{
    public static event Action onJobSchedule;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped in ScheduleSlot"); 
        if (eventData.pointerDrag != null)
        {
            // Parent job to the schedule slot if dropped inside 
            eventData.pointerDrag.transform.parent = gameObject.transform;

            onJobSchedule?.Invoke();
        }
    }
}
