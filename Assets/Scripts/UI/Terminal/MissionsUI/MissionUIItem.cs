﻿using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionUIItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [Header("Set in Editor")]
    public Text MissionNameText;

    [Header("Set at Runtime")]
    public Mission Mission;
    private MissionsUI missionsUI;
    private MissionDetailsUI missionDetailsUI;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform myRectTransform;

    private void Awake()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
        missionDetailsUI = GetComponentInParent<MissionDetailsUI>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        myRectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable() => gameObject.DestroyIfExists();

    public void Init(Mission mission)
    {
        Mission = mission;
        MissionNameText.SetText(mission.Name, FontType.ListItem);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Mission.IsInProgress())
        {
            myRectTransform.SetParent(missionsUI.transform);
            canvasGroup.alpha = MissionConstants.DragAlpha;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Delta is the distance that the mouse moved since previous frame
        // Divide by canvas scale factor to prevent object from not following the mouse properly on a scaled canvas
        if (!Mission.IsInProgress())
        {
            myRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Mission.IsInProgress())
        {
            // Raycast will pass through and hit the schedule
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = MissionConstants.DropAlpha;

            MissionScheduleSlot scheduleSlot = missionsUI.GetSlotByPosition(eventData.position);
            if (scheduleSlot != null)
            {
                SingletonManager.EventService.Dispatch<OnMissionSlottedEvent>();

                CheckReplaceMission(scheduleSlot);
                myRectTransform.SetParent(scheduleSlot.layoutContainer);
                myRectTransform.SetSiblingIndex(0);

                if (scheduleSlot.GetComponentInChildren<PilotInMissionScheduleSlot>() != null)
                {
                    // Schedule a mission if there is already a pilot in the slot 
                    Schedule(scheduleSlot);
                }
                else
                {
                    // Open the pilot select menu after dropping a mission into a slot that has no pilot in it
                    missionsUI.PopulatePilotSelect(scheduleSlot, Mission);
                }
            }
            else
            {
                // Unschedule the mission if it is dropped outside a slot
                Unschedule(scheduleSlot);
            }
        }
    }

    private void CheckReplaceMission(MissionScheduleSlot scheduleSlot)
    {
        if (scheduleSlot.layoutContainer.childCount > 0)
        {
            MissionUIItem itemToReplace = scheduleSlot.GetComponentInChildren<MissionUIItem>();
            if (itemToReplace != null)
            {
                itemToReplace.Unschedule(scheduleSlot);
            }
        }
    }

    public void Schedule(MissionScheduleSlot scheduleSlot)
    {
        Pilot pilot = HangarManager.GetPilotByNode(scheduleSlot.hangarNode);
        if (pilot != null && Mission != null)
        {
            MissionsManager.AddOrUpdateScheduledMission(pilot, Mission);
        }
    }

    public void Unschedule(MissionScheduleSlot scheduleSlot = null)
    {
        MissionsManager.RemoveScheduledMission(Mission);
        gameObject.Orphan();
        missionsUI.PopulateMissionSelect();
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            MissionScheduleSlot scheduleSlot = missionsUI.GetSlotByPosition(eventData.position);
            if (scheduleSlot != null && scheduleSlot.IsActive)
            {
                // Reset the schedule slot if the player clicks and there is already one scheduled
                Unschedule();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (missionDetailsUI.missionBeingDisplayed != Mission)
            {
                // Show the mission details for the scheduled mission 
                missionDetailsUI.missionBeingDisplayed = Mission;
                missionDetailsUI.DisplayMissionDetails(this);
            }
            else
            {
                // Hide the mission details if they are already visible 
                missionDetailsUI.DestroyMissionDetails();
                missionDetailsUI.missionBeingDisplayed = null;
            }
        }
    }
}
