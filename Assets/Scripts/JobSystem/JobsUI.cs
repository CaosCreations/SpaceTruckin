using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class JobsUI : MonoBehaviour
{
    public GameObject mainCanvas; 
    public GameObject scheduleSlotPrefab; 
    public JobsContainer jobsContainer;

    private GameObject availableJobsContainer;
    private GameObject scheduleContainer;

    private Sprite containerBackgroundSprite;
    private Sprite scheduleSlotSprite; 
    private Sprite jobSprite;

    private int scheduleSlots = JobConstants.scheduleSlots; 

    private void Start()
    {
        Debug.Log("JobsContainer length: " + jobsContainer.jobsContainer.Length);

        // Load sprites 
        containerBackgroundSprite = Resources.Load<Sprite>(JobConstants.uiBackgroundSpritePath);
        scheduleSlotSprite = Resources.Load<Sprite>(JobConstants.scheduleSlotSpritePath);
        jobSprite = Resources.Load<Sprite>(JobConstants.jobSpritePath);

        // Create UI elements and populate with jobs 
        availableJobsContainer = InitialiseAvailableJobsContainer();
        scheduleContainer = InitialiseScheduleContainer();
        InitialiseScheduleSlots(); 
        AddAcceptedJobs();

        //MessageDetailView.onJobAccept += 
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

        return containerObject; 
    }

    private GameObject InitialiseScheduleContainer()
    {
        GameObject scheduleObject = new GameObject(JobConstants.scheduleContainerName);
        scheduleObject.transform.parent = gameObject.transform;

        Image scheduleImage = scheduleObject.AddComponent<Image>();
        scheduleImage.sprite = containerBackgroundSprite;

        scheduleObject.AddComponent<CanvasGroup>().blocksRaycasts = true; 
        //scheduleObject.AddComponent<ScheduleUI>(); 

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

    // Jobs show up in the jobs container only when 
    // they've been accepted in the messages UI 
    //
    // Jobs are "available" when they have been offered
    // via a message but haven't been "accepted" yet 
    private void AddAcceptedJobs()
    {
        foreach (Job job in jobsContainer.jobsContainer)
        {
            if (job.isAccepted)
            {
                GameObject newJob = new GameObject(job.title); 
                newJob.transform.parent = availableJobsContainer.transform;
                job.draggableJobObject = newJob; 

                // Store location data for repositioning on failed drop.  
                // Assign the individual floats to prevent copying the reference. 
                job.startingPosition = new Vector2(newJob.transform.position.x, newJob.transform.position.y);

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
        }
    }

    private void InitialiseScheduleSlots()
    {
        for (int i = 1; i <= scheduleSlots; i++)
        {
            GameObject scheduleSlot = Instantiate(scheduleSlotPrefab, scheduleContainer.transform);
            scheduleSlot.name = "ScheduleSlot";
            scheduleSlot.GetComponentInChildren<Text>().text = i.ToString(); 
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
