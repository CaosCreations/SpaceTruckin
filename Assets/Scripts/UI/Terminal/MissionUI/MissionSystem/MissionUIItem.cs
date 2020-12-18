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
        myRectTransform.SetParent(missionsUI.transform);
        canvasGroup.alpha = MissionConstants.dragAlpha;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Delta is the distance that the mouse moved since previous frame
        // Divide by canvas scale factor to prevent object from not following the mouse properly on a scaled canvas
        myRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Raycast will pass through and hit the schedule
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = MissionConstants.dropAlpha;

        MissionScheduleSlot slot = missionsUI.GetSlotForMissionDrag(eventData.position);
        if(slot != null)
        {
            myRectTransform.SetParent(slot.slotTransform);
            mission.scheduledHangarNode = slot.hangarNode;
        }
        else
        {
            myRectTransform.SetParent(scrollViewContent);
            mission.scheduledHangarNode = HangarNode.None;
        }
    }
}
