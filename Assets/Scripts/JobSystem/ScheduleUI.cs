using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScheduleUI : MonoBehaviour, IDropHandler
{
    public static event Action onJobSchedule;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // Parent job to the schedule if dropped inside 
            eventData.pointerDrag.transform.parent = gameObject.transform;

            onJobSchedule?.Invoke();
        }
    }
}
