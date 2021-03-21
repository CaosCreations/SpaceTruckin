using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject pilotSelectItemPrefab;
    public Button pilotSelectCloseButton;
    private MissionDetailsUI missionDetailsUI;

    public MissionScheduleSlot[] scheduleSlots;

    private void Start()
    {
        missionDetailsUI = GetComponent<MissionDetailsUI>();
    }

    private void OnEnable()
    {
        SetActiveSlots();
        PopulateScheduleSlots();
        PopulateMissionSelect();
    }

    private void SetActiveSlots()
    {
        foreach (MissionScheduleSlot slot in scheduleSlots)
        {
            if (slot != null)
            {
                slot.IsActive = HangarManager.NodeIsUnlocked(slot.hangarNode);
            }
        }
    }

    private void CleanMenu()
    {
        scrollViewContent.transform.DestroyDirectChildren();

        foreach (MissionScheduleSlot slot in scheduleSlots)
        {
            if(slot.layoutContainer.childCount > 0)
            {
                Destroy(slot.layoutContainer.GetChild(0).gameObject);
            }
        }

        if (missionDetailsUI != null)
        {
            missionDetailsUI.DestroyMissionDetails();
        }
    }

    public void PopulateMissionSelect()
    {
        scrollViewContent.transform.DestroyDirectChildren();
        List<Mission> acceptedMissions = MissionsManager.GetAcceptedMissions();
        if (acceptedMissions != null)
        {
            foreach (Mission mission in acceptedMissions)
            {
                if (mission != null)
                {
                    GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
                    MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                    missionItem.Init(mission, scrollViewContent.transform);
                }
            }
        }
    }

    public void PopulatePilotSelect(MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        scrollViewContent.transform.DestroyDirectChildren();
        Pilot[] availablePilots = PilotsManager.GetPilotsAvailableForMissions();
        if (availablePilots != null)
        {
            foreach (Pilot pilot in availablePilots)
            {
                if (pilot != null)
                {
                    GameObject pilotSelectItem = Instantiate(pilotSelectItemPrefab, scrollViewContent.transform);
                    pilotSelectItem.GetComponent<PilotSelectItem>().Init(pilot, scheduleSlot, mission);
                }
            }
        }
    }

    private void PopulateScheduleSlots()
    {
        foreach (MissionScheduleSlot scheduleSlot in scheduleSlots)
        {
            if (scheduleSlot != null)
            {
                scheduleSlot.CleanSlot();

                ScheduledMission scheduled = MissionsManager.GetScheduledMissionAtSlot(scheduleSlot.hangarNode);
                if (scheduled != null)
                {
                    // Put a mission in the schedule slot if it hasn't been started yet 
                    if (scheduled.Mission != null)
                    {
                        GameObject missionItemInstance = Instantiate(missionItemPrefab, scheduleSlot.layoutContainer);
                        missionItemInstance.GetComponent<MissionUIItem>().Init(scheduled.Mission, scrollViewContent.transform);
                    }
                }

                HangarSlot hangarSlot = HangarManager.GetSlotByNode(scheduleSlot.hangarNode); // Reorder
                if (HangarManager.ShipIsDockedAtSlot(hangarSlot))
                {
                    // Put a pilot in the schedule slot if its ship is still docked  
                    if (hangarSlot.Ship != null)
                    {
                        scheduleSlot.PutPilotInSlot(hangarSlot.Ship.Pilot);
                    }
                }
            }
        }
    }

    private void ShowMissionProgress(MissionUIItem missionItem)
    {
        string substring = missionItem.mission.DaysLeftToComplete > 1 ? "days" : "day";

        missionItem.missionNameText.SetText(missionItem.mission.Name 
            + $"\n({missionItem.mission.DaysLeftToComplete} {substring} remaining)", FontType.ListItem);
    }

    public MissionScheduleSlot GetSlotByPosition(Vector2 position)
    {
        foreach (MissionScheduleSlot slot in scheduleSlots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(slot.parentTransform, position)
                && slot.IsActive)
            {
                return slot;
            }
        }
        return null;
    }
}
