using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public GameObject missionsPanel;

    public GameObject availableJobsContainer;
    public GameObject scheduleContainer;

    private MissionsUI jobsUI;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Start()
    {
        jobsUI = missionsPanel.GetComponent<MissionsUI>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Unparent the job object from the available jobs UI 
        eventData.selectedObject.transform.SetParent(missionsPanel.transform);

        canvasGroup.alpha = MissionConstants.dragAlpha;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Delta is the distance that the mouse moved since previous frame
        // Divide by canvas scale factor to prevent object from not following the mouse properly on a scaled canvas
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // This seems to be called before the Scehdule's OnDrop method
    public void OnEndDrag(PointerEventData eventData)
    {
        // Raycast will pass through and hit the schedule 
        canvasGroup.blocksRaycasts = true;

        canvasGroup.alpha = MissionConstants.dropAlpha;

        // Return to starting position if not dropped in a slot  
        //if (!jobsUI.IsInsideSlot(gameObject))
        //{
        //    gameObject.transform.parent = availableJobsContainer.transform;

        //    // If the job was previously scheduled, update the schedule accordingly 
        //    if (job.isScheduled)
        //    {
        //        onUnscheduleJob?.Invoke(job);
        //    }
        //}
    }
}
