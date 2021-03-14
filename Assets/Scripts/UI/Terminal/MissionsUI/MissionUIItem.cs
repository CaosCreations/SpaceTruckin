using UnityEngine;
using UnityEngine.Events;
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
        if(!mission.IsInProgress()) // Update condition for new hangar requirements?
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
                myRectTransform.SetParent(scheduleSlot.slotTransform);

                // Open the pilot select menu after dropping a mission into a slot
                missionsUI.PopulatePilotSelect(mission, scheduleSlot);
            }
            else
            {
                Unschedule(scheduleSlot);
            }
        }
    }

    private void CheckReplaceMission(MissionScheduleSlot scheduleSlot)
    {
        if (scheduleSlot.slotTransform.childCount > 0)
        {
            MissionUIItem missionToUnschedule = scheduleSlot.slotTransform.GetChild(0).GetComponent<MissionUIItem>();
            
            if (missionToUnschedule != null)
            {
                missionToUnschedule.Unschedule(scheduleSlot);
            }
        }
    }

    public void Unschedule(MissionScheduleSlot scheduleSlot)
    {
        myRectTransform.SetParent(scrollViewContent);
        if (mission.Pilot != null)
        {
            mission.Pilot.CurrentMission = null; // Get directly from missions manager
            mission.Pilot = null;
        }

        // Remove the ship object from the hangar if it is unscheduled
        HangarSlot hangarSlot = HangarManager.GetSlotByNode(scheduleSlot.hangarNode);
        HangarManager.Instance.DestroyShipInstance(hangarSlot);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            MissionScheduleSlot scheduleSlot = missionsUI.GetSlotByPosition(eventData.position);
            if (scheduleSlot != null)
            {
                if (scheduleSlot.Pilot == null)
                {
                    // Allow the player to dock a ship at a node without dragging on a mission
                    missionsUI.PopulatePilotSelect(mission, scheduleSlot);
                }
                else
                {
                    // Reset the schedule slot if the player clicks and there is one scheduled
                    Unschedule(scheduleSlot);
                }
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
