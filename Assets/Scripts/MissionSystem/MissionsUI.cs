using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;

    //public RectTransform[] missionSlots;
    public MissionScheduleSlot[] missionSlots;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        CleanScrollView();
        PopulateScrollView();
    }

    void CleanScrollView()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void PopulateScrollView()
    {
        foreach (Mission mission in MissionsManager.Instance.missionsAcceptedInNoticeBoard)
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission);
        }
    }

    public Transform GetSlotForMissionDrag(Vector2 position)
    {
        foreach(MissionScheduleSlot slot in missionSlots)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(slot.parentTransform, position))
            {
                return slot.slotTransform;
            }
        }

        return null;
    }
}
