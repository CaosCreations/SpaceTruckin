using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static event Action<Job> onUnscheduleJob;

    public GameObject mainCanvas;
    public GameObject jobsPanel;
    public GameObject availableJobsContainer; 
    public GameObject scheduleContainer;
    public GameObject testSchedule; 
    public Job job;

    private JobsUI jobsUI;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Start()
    {
        jobsUI = jobsPanel.GetComponent<JobsUI>(); 
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Unparent the job object from the available jobs UI 
        gameObject.transform.parent = jobsPanel.transform;

        canvasGroup.alpha = JobConstants.dragAlpha; 
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Delta is the distance that the mouse moved since previous frame
        // Divide by canvas scale factor to prevent object from overshooting 
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.GetComponent<Canvas>().scaleFactor;
    }

    // This seems to be called before the Scehdule's OnDrop method
    public void OnEndDrag(PointerEventData eventData)
    {
        // Raycast will pass through and hit the schedule 
        canvasGroup.blocksRaycasts = true;

        canvasGroup.alpha = JobConstants.dropAlpha; 

        // Return to starting position if not dropped in a slot  
        if (!jobsUI.IsInsideSlot(gameObject))
        {
            gameObject.transform.parent = availableJobsContainer.transform;

            // If the job was previously scheduled, update the schedule accordingly 
            if (job.isScheduled)
            {
                onUnscheduleJob?.Invoke(job);
            }
        }        
    }

    private bool IsInsideSchedule()
    {
        RectTransform scheduleRectTransform = scheduleContainer.GetComponent<RectTransform>();

        return rectTransform.rect.x < scheduleRectTransform.rect.x + scheduleRectTransform.rect.width &&
            scheduleRectTransform.rect.x < rectTransform.rect.x + rectTransform.rect.width &&
            rectTransform.rect.y < scheduleRectTransform.rect.y + scheduleRectTransform.rect.height &&
            scheduleRectTransform.rect.y < rectTransform.rect.y + rectTransform.rect.height; 
    }
}
