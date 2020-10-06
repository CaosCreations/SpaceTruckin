﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JobsUI : MonoBehaviour
{
    public GameObject mainCanvas; 
    public GameObject scheduleSlotPrefab; 
    public JobsContainer jobsContainer;
    public Schedule schedule;
    public JobsManager jobsManager; 

    private GameObject availableJobsContainer;
    private GameObject scheduleContainer;

    private Sprite containerBackgroundSprite;
    private Sprite scheduleSlotSprite; 
    private Sprite jobSprite;

    private void Start()
    {
        // Load sprites 
        containerBackgroundSprite = Resources.Load<Sprite>(JobConstants.containerBackgroundSpritePath);
        scheduleSlotSprite = Resources.Load<Sprite>(JobConstants.scheduleSlotSpritePath);
        jobSprite = Resources.Load<Sprite>(JobConstants.jobSpritePath);

        // Create UI elements and populate with jobs 
        scheduleContainer = InitialiseScheduleContainer();
        availableJobsContainer = InitialiseAvailableJobsContainer();
        InitialiseScheduleSlots(); 
        AddJobs();

        // Updates available jobs container when job is accepted from a message  
        MessageDetailView.onJobAccept += AddJob;

        schedule.LogSchedule(); 
    }

    private GameObject InitialiseAvailableJobsContainer()
    {
        GameObject containerObject = new GameObject(JobConstants.availableJobsContainerName);
        containerObject.transform.parent = gameObject.transform;

        Image containerImage = containerObject.AddComponent<Image>();
        containerImage.sprite = containerBackgroundSprite;

        RectTransform rectTransform = containerObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = JobConstants.availableJobsContainerAnchorMin;
        rectTransform.anchorMax = JobConstants.availableJobsContainerAnchorMax;
        rectTransform.localPosition = Vector2.left; 
        rectTransform.localScale = new Vector2(0.75f, 0.75f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        JobsUtils.SetupGridLayoutGroup(containerObject);

        // For now, this container requires more spacing 
        containerObject.GetComponent<GridLayoutGroup>().spacing = JobConstants.availableJobsGridSpacing;

        return containerObject; 
    }

    private GameObject InitialiseScheduleContainer()
    {
        GameObject scheduleObject = new GameObject(JobConstants.scheduleContainerName);
        scheduleObject.transform.parent = gameObject.transform;

        Image scheduleImage = scheduleObject.AddComponent<Image>();
        scheduleImage.sprite = containerBackgroundSprite;

        scheduleObject.AddComponent<CanvasGroup>().blocksRaycasts = true; 

        RectTransform rectTransform = scheduleObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(0.5f, 0f);
        rectTransform.anchorMin = JobConstants.scheduleContainerAnchorMin;
        rectTransform.anchorMax = JobConstants.scheduleContainerAnchorMax;
        rectTransform.localScale = JobConstants.scheduleContainerLocalScale;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        JobsUtils.SetupGridLayoutGroup(scheduleObject); 

        return scheduleObject;
    }

    private void AddJob(Job job)
    {
        GameObject newJob = new GameObject(job.title);

        // If job was already scheduled, parent it to its pre-assigned schedule slot,
        // which matches the date it was scheduled for. 
        Transform parentTransform = job.isScheduled ?
            job.scheduleSlotTransform : availableJobsContainer.transform;

        newJob.transform.parent = parentTransform;

        Image jobImage = newJob.AddComponent<Image>();
        jobImage.sprite = jobSprite;

        DragNDrop dragNDrop = newJob.AddComponent<DragNDrop>();
        dragNDrop.mainCanvas = mainCanvas;
        dragNDrop.jobsPanel = transform.gameObject;
        dragNDrop.availableJobsContainer = availableJobsContainer;
        dragNDrop.scheduleContainer = scheduleContainer;
        dragNDrop.job = job;

        // Needs to be false otherwise Schedule can't fire events. 
        newJob.AddComponent<CanvasGroup>().interactable = false;
    }

    // Jobs show up in the jobs container only when 
    // they've been accepted in the messages UI 
    //
    // Jobs are "available" when they have been offered
    // via a message but haven't been "accepted" yet 
    private void AddJobs()
    {
        foreach (Job job in jobsContainer.jobsContainer)
        {
            if (job.isAccepted)
            {
                AddJob(job); 
            }
        }
    }

    private void InitialiseScheduleSlots()
    {
        for (int i = 0; i < schedule.numberOfDays; i++)
        {
            GameObject scheduleSlot = Instantiate(scheduleSlotPrefab, scheduleContainer.transform);
            int dayOfMonth = i + 1;
            scheduleSlot.name = "ScheduleSlot" + dayOfMonth.ToString();
            scheduleSlot.GetComponent<ScheduleSlot>().dayOfMonth = dayOfMonth;
            scheduleSlot.GetComponentInChildren<Text>().text = dayOfMonth.ToString(); 

            if (schedule.schedule.ContainsKey(dayOfMonth))
            {
                // We don't have a reference to the schedule slot from last session,
                // since it's been rebuilt 
                schedule.schedule[i].scheduleSlotTransform = scheduleSlot.transform;
            }
        }
    }

    public bool IsInsideSlot(GameObject draggableJob)
    {
        foreach (Transform _transform in scheduleContainer.transform)
        {
            if (draggableJob.transform.parent == _transform)
            {
                return true; 
            }
        }
        return false; 
    }
}
