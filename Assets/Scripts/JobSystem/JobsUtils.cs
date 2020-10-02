using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobsUtils : MonoBehaviour
{
    public static void SetupGridLayoutGroup(GameObject _gameObject)
    {
        GridLayoutGroup gridLayoutGroup = _gameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.padding.top = JobConstants.gridTopPadding;
        gridLayoutGroup.padding.left = JobConstants.gridLeftPadding;
        gridLayoutGroup.cellSize = JobConstants.gridCellSize;
        gridLayoutGroup.spacing = JobConstants.gridSpacing; 
    }

    /*
        Run-time Jobs UI generation:  
    */
    public static void InitialiseScheduleSlots(Schedule schedule, GameObject scheduleContainer, GameObject scheduleSlotPrefab)
    {
        for (int i = 1; i <= schedule.numberOfDays; i++)
        {
            if (schedule.schedule.ContainsKey(i))
            {
                // Don't rebuild the schedule slot if it already exists, 
                // i.e. the job has already been scheduled, 
                // just set it's parent since the container is being rebuilt 
                GameObject scheduleSlot = schedule.schedule[i].scheduleSlotTransform.gameObject;
                scheduleSlot.transform.parent = scheduleContainer.transform;

                // Problem: how to order them correctly in the grid layout group 
            }
            else
            {
                GameObject scheduleSlot = Instantiate(scheduleSlotPrefab, scheduleContainer.transform);
                scheduleSlot.name = "ScheduleSlot" + i.ToString();
                scheduleSlot.GetComponent<ScheduleSlot>().dayOfMonth = i;
                scheduleSlot.GetComponentInChildren<Text>().text = i.ToString();
            }
        }
    }
    private GameObject InitialiseAvailableJobsContainer(Transform parentTransform, Sprite sprite)
    {
        GameObject containerObject = new GameObject(JobConstants.availableJobsContainerName);
        containerObject.transform.parent = parentTransform;

        Image containerImage = containerObject.AddComponent<Image>();
        containerImage.sprite = sprite;

        RectTransform rectTransform = containerObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = JobConstants.availableJobsContainerAnchorMin;
        rectTransform.anchorMax = JobConstants.availableJobsContainerAnchorMax;
        rectTransform.localPosition = Vector2.left;
        rectTransform.localScale = new Vector2(0.75f, 0.75f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        SetupGridLayoutGroup(containerObject);

        return containerObject;
    }

    public static GameObject InitialiseScheduleContainer(Transform parentTransform, Sprite sprite)
    {
        GameObject scheduleObject = new GameObject(JobConstants.scheduleContainerName);
        scheduleObject.transform.parent = parentTransform;

        Image scheduleImage = scheduleObject.AddComponent<Image>();
        scheduleImage.sprite = sprite;

        scheduleObject.AddComponent<CanvasGroup>().blocksRaycasts = true;

        RectTransform rectTransform = scheduleObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(0.5f, 0f);
        rectTransform.anchorMin = JobConstants.scheduleContainerAnchorMin;
        rectTransform.anchorMax = JobConstants.scheduleContainerAnchorMax;
        rectTransform.localScale = JobConstants.scheduleContainerLocalScale;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        SetupGridLayoutGroup(scheduleObject);

        return scheduleObject;
    }

}
