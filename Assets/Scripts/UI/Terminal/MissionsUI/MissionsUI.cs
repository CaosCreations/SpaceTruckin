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
        List<Mission> selectableMissions = MissionsManager.GetSelectableMissions();
        if (!selectableMissions.IsNullOrEmpty())
        {
            missionsUnavailableText.DestroyIfExists();

            foreach (Mission mission in selectableMissions)
            {
                if (mission != null)
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
        pilotsUnavailableText.DestroyIfExists();
        
        if (!PilotsManager.PilotsInQueue.IsNullOrEmpty())
        {
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
