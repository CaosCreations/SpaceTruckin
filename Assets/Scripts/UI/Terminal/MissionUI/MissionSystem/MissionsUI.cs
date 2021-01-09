using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    private MissionDetailsUI missionDetailsUI;

    public MissionScheduleSlot[] missionSlots;

    void Start()
    {
        missionDetailsUI = GetComponent<MissionDetailsUI>();
    }

    private void OnEnable()
    {
        SetupMissionSlots();
        CleanMenu();
        PopulateScrollView();
    }

    void SetupMissionSlots()
    {
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            Ship ship = ShipsManager.NodeHasShip(slot.hangarNode);
            if (ship != null)
            {
                slot.ship = ship;
                if(ship.shipSaveData.currentMission == null)
                {
                    slot.IsActive = true;
                }
                else
                {
                    slot.IsActive = !ship.shipSaveData.currentMission.IsInProgress();
                }
            }
            else
            {
                slot.IsActive = false;
            }
        }
    }

    void CleanMenu()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach(MissionScheduleSlot slot in missionSlots)
        {
            if(slot.slotTransform.childCount > 0)
            {
                Destroy(slot.slotTransform.GetChild(0).gameObject);
            }
        }
        missionDetailsUI?.DestroyMissionDetails();
    }

    void PopulateScrollView()
    {
        foreach (Mission mission in MissionsManager.GetAcceptedMissions())
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission, scrollViewContent.transform);
        }

        List<Mission> scheduledMissions = MissionsManager.GetScheduledMissions();
        foreach(MissionScheduleSlot slot in missionSlots)
        {
            Mission missionForSlot = scheduledMissions.Where(x => x.missionSaveData.ship == slot.ship).FirstOrDefault();

            if(missionForSlot != null)
            {
                GameObject scrollItem = Instantiate(missionItemPrefab, slot.slotTransform);
                MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                missionItem.Init(missionForSlot, scrollViewContent.transform);
                
                if (missionItem.mission.IsInProgress())
                {
                    ShowMissionProgress(missionItem);
                }
            }
        }
    }

    private void ShowMissionProgress(MissionUIItem missionItem)
    {
        string substring = missionItem.mission.missionSaveData.daysLeftToComplete > 1 ? "days" : "day";
        
        missionItem.missionNameText.text = missionItem.mission.missionName 
            + $"\n({missionItem.mission.missionSaveData.daysLeftToComplete} {substring} remaining)";
    }

    public MissionScheduleSlot GetSlotForMissionDrag(Vector2 position)
    {
        foreach(MissionScheduleSlot slot in missionSlots)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(slot.parentTransform, position)
                && slot.IsActive)
            {
                return slot;
            }
        }

        return null;
    }
}
