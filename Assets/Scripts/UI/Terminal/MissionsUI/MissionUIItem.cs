using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionUIItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [Header("Set in Editor")]
    public Text missionNameText;

    [Header("Set at Runtime")]
    public Mission mission;
    private MissionsUI missionsUI;
    private MissionDetailsUI missionDetailsUI;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform myRectTransform;
    private Transform scrollViewContent;

    private void Awake()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
        missionDetailsUI = GetComponentInParent<MissionDetailsUI>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        myRectTransform = GetComponent<RectTransform>();
    }

    public void Init(Mission mission, Transform scrollViewContent)
    {
        this.mission = mission;
        missionNameText.SetText(mission.Name, FontType.ListItem);
        this.scrollViewContent = scrollViewContent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!mission.IsInProgress())
        {
            myRectTransform.SetParent(missionsUI.transform);
            canvasGroup.alpha = MissionConstants.dragAlpha;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Delta is the distance that the mouse moved since previous frame
        // Divide by canvas scale factor to prevent object from not following the mouse properly on a scaled canvas
        if (!mission.IsInProgress())
        {
            myRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!mission.IsInProgress())
        {
            // Raycast will pass through and hit the schedule
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = MissionConstants.dropAlpha;

            MissionScheduleSlot scheduleSlot = missionsUI.GetSlotByPosition(eventData.position);
            if (scheduleSlot != null)
            {
                CheckReplaceMission(scheduleSlot);
                myRectTransform.SetParent(scheduleSlot.layoutContainer);

                if (scheduleSlot.CurrentPilotItemInSlot != null)
                {
                    // Schedule a mission if there is already a pilot in the slot 
                    Schedule(scheduleSlot);
                }
                else
                {
                    // Open the pilot select menu after dropping a mission into a slot that has no pilot in it
                    missionsUI.PopulatePilotSelect(scheduleSlot, mission);
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
            //MissionUIItem missionToUnschedule = scheduleSlot.layoutContainer.GetChild(0).GetComponent<MissionUIItem>();
            //MissionUIItem missionToUnschedule = scheduleSlot.missionLayoutContainer.GetChild(1).GetComponent<MissionUIItem>();

            if (scheduleSlot.CurrentMissionItemInSlot != null)
            {
                scheduleSlot.CurrentMissionItemInSlot.Unschedule(scheduleSlot);
            }
        }
    }

    public void Schedule(MissionScheduleSlot scheduleSlot)
    {
        Pilot pilot = scheduleSlot.CurrentPilotItemInSlot.pilot;
        if (pilot != null && mission != null)
        {
            MissionsManager.AddOrUpdateScheduledMission(pilot, mission);
        }
    }

    public void Unschedule(MissionScheduleSlot scheduleSlot = null)
    {
        MissionsManager.RemoveScheduledMission(mission);
        Destroy(gameObject);

        if (scheduleSlot != null)
        {
            scheduleSlot.IsActive = true;
        }
        missionsUI.PopulateMissionSelect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            MissionScheduleSlot scheduleSlot = missionsUI.GetSlotByPosition(eventData.position);
            if (scheduleSlot != null && HangarManager.NodeIsUnlocked(scheduleSlot.hangarNode))
            {
                // Reset the schedule slot if the player clicks and there is already one scheduled
                Unschedule();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (missionDetailsUI.missionBeingDisplayed != mission)
            {
                // Show the mission details for the scheduled mission 
                missionDetailsUI.missionBeingDisplayed = mission;
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
