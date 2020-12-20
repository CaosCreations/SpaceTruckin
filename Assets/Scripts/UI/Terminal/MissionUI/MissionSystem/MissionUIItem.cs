using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionUIItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Set in Editor")]
    public Text missionNameText;

    [Header("Set at Runtime")]
    public Mission mission;
    private MissionsUI missionsUI;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform myRectTransform;
    private Transform scrollViewContent;

    private void Awake()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        myRectTransform = GetComponent<RectTransform>();
    }

    public void Init(Mission mission, Transform scrollViewContent)
    {
        this.mission = mission;
        missionNameText.text = mission.missionName;
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

            MissionScheduleSlot slot = missionsUI.GetSlotForMissionDrag(eventData.position);
            if (slot != null)
            {
                CheckReplaceMission(slot);
                myRectTransform.SetParent(slot.slotTransform);

                if (slot.ship != null)
                {
                    slot.ship.currentMission = mission;
                    mission.ship = slot.ship;
                }
                else
                {
                    Debug.LogError("The MissionScheduleSlot does not have a ship");
                }
            }
            else
            {
                Unschedule();
            }
        }
    }

    private void CheckReplaceMission(MissionScheduleSlot slot)
    {
        if(slot.slotTransform.childCount > 0)
        {
            MissionUIItem missionToUnschedule = slot.slotTransform.GetChild(0).GetComponent<MissionUIItem>();
            
            if(missionToUnschedule != null)
            {
                missionToUnschedule.Unschedule();
            }
        }
    }

    public void Unschedule()
    {
        myRectTransform.SetParent(scrollViewContent);
        mission.ship.currentMission = null;
        mission.ship = null;
    }
}
