using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject pilotSelectItemPrefab;
    public Button pilotSelectCloseButton;

    public GameObject missionsUnavailablePrefab;
    public GameObject pilotsUnavailablePrefab;
    private GameObject missionsUnavailableText;
    private GameObject pilotsUnavailableText;

    public MissionScheduleSlot[] scheduleSlots;

    private void OnEnable()
    {
        SetActiveSlots();
        PopulateScheduleSlots();
        PopulateMissionSelect();
        pilotSelectCloseButton.SetActive(false);
    }

    private void Start()
    {
        pilotSelectCloseButton.AddOnClick(PopulateMissionSelect);
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

    public void PopulateMissionSelect()
    {
        scrollViewContent.transform.DestroyDirectChildren();
        pilotSelectCloseButton.SetActive(false);

        List<Mission> selectableMissions = MissionsManager.GetSelectableMissions();
        if (!selectableMissions.IsNullOrEmpty())
        {
            missionsUnavailableText.DestroyIfExists();

            foreach (Mission mission in selectableMissions)
            {
                if (mission != null && !MissionIsInAScheduleSlot(mission))
                {
                    GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
                    MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                    missionItem.Init(mission);
                }
            }
        }
        else
        {
            // Show a message saying there are no missions available
            missionsUnavailableText = Instantiate(missionsUnavailablePrefab, scrollViewContent.transform);
        }
    }

    public void PopulatePilotSelect(MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        scrollViewContent.transform.DestroyDirectChildren();
        
        if (!PilotsManager.PilotsInQueue.IsNullOrEmpty())
        {
            pilotsUnavailableText.DestroyIfExists();
            foreach (Pilot pilot in PilotsManager.PilotsInQueue)
            {
                if (pilot != null)
                {
                    GameObject pilotSelectItem = Instantiate(pilotSelectItemPrefab, scrollViewContent.transform);
                    pilotSelectItem.GetComponent<PilotSelectItem>().Init(pilot, scheduleSlot, mission);
                }
            }
        }
        else
        {
            // Show a message saying there are no pilots available
            pilotsUnavailableText = Instantiate(pilotsUnavailablePrefab, scrollViewContent.transform);
        }

        pilotSelectCloseButton.SetActive(true);
    }

    private void PopulateScheduleSlots()
    {
        foreach (MissionScheduleSlot scheduleSlot in scheduleSlots)
        {
            if (scheduleSlot != null)
            {
                scheduleSlot.CleanSlot();

                ScheduledMission scheduled = MissionsManager.GetScheduledMission(scheduleSlot.hangarNode);
                if (scheduled != null)
                {
                    // Put a mission in the schedule slot if it hasn't been started yet 
                    if (scheduled.Mission != null)
                    {
                        GameObject missionItemInstance = Instantiate(missionItemPrefab, scheduleSlot.layoutContainer);
                        missionItemInstance.GetComponent<MissionUIItem>().Init(scheduled.Mission);
                    }
                }

                HangarSlot hangarSlot = HangarManager.GetSlotByNode(scheduleSlot.hangarNode);
                if (HangarManager.ShipIsDocked(hangarSlot))
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
        string substring = missionItem.Mission.DaysLeftToComplete > 1 ? "days" : "day";

        missionItem.MissionNameText.SetText(missionItem.Mission.Name
            + $"\n({missionItem.Mission.DaysLeftToComplete} {substring} remaining)", FontType.ListItem);
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

    private bool MissionIsInAScheduleSlot(Mission mission)
    {
        foreach (MissionScheduleSlot scheduleSlot in scheduleSlots)
        {
            MissionUIItem itemInSlot = scheduleSlot.GetComponentInChildren<MissionUIItem>();
            if (itemInSlot != null && itemInSlot.Mission == mission)
            {
                return true;
            }
        }
        return false;
    }
}
