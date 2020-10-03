using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JobsUI : MonoBehaviour
{
    public GameObject mainCanvas; 
    public GameObject availableJobsContainer;
    public GameObject scheduleContainer;
    public GameObject scheduleSlotPrefab;
    public GameObject jobPrefab;

    public Schedule schedule;
    public ScheduleSlot[] scheduleSlots; 

    public JobsContainer jobsContainer;

    private void Start()
    {
        SetupScheduleSlots(); 
        AddJobs();

        // Updates available jobs container when job is accepted from a message  
        MessageDetailView.onJobAccept += AddJob; 
    }

    private void AddJob(Job job)
    {
        GameObject newJob = Instantiate(jobPrefab);
        newJob.name = job.title;

        // If job was already scheduled, parent it to its pre-assigned schedule slot,
        // which matches the date it was scheduled for. 
        Transform parentTransform = job.isScheduled ? 
            job.scheduleSlotTransform : availableJobsContainer.transform;

        newJob.transform.parent = parentTransform;

        DragNDrop dragNDrop = newJob.GetComponent<DragNDrop>();
        dragNDrop.mainCanvas = mainCanvas;
        dragNDrop.jobsPanel = transform.gameObject;
        dragNDrop.availableJobsContainer = availableJobsContainer;
        dragNDrop.scheduleContainer = scheduleContainer;
        dragNDrop.job = job;
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

    private void SetupScheduleSlots()
    {
        for (int i = 0; i < scheduleSlots.Length; i++)
        {
            int dayOfMonth = i + 1;
            scheduleSlots[i].GetComponent<ScheduleSlot>().dayOfMonth = dayOfMonth;
            scheduleSlots[i].GetComponentInChildren<Text>().text = dayOfMonth.ToString(); 
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
