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
        foreach (Mission mission in MissionsManager.GetAcceptedMissions())
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission, scrollViewContent.transform);
        }
    }

    public void PopulatePilotSelect(MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        scrollViewContent.transform.DestroyDirectChildren();
        Pilot[] pilotsToSelect = PilotsManager.GetPilotsAvailableForMissions();
        if (pilotsToSelect != null)
        {
            foreach (Pilot pilot in pilotsToSelect)
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
        List<ScheduledMission> scheduledMissions = MissionsManager.GetScheduledMissionsNotInProgress();
        foreach (MissionScheduleSlot scheduleSlot in scheduleSlots)
        {
            if (scheduleSlot != null)
            {
                scheduleSlot.CleanSlot();

                ScheduledMission scheduled = scheduledMissions.FirstOrDefault(x => x == scheduleSlot.ScheduledMission); // Encapsulate?
                if (scheduled != null)
                {
                    // Put a mission in the slot if its ship has not been launched yet
                    if (scheduled.Mission != null)
                    {
                        GameObject missionItemInstance = Instantiate(missionItemPrefab, scheduleSlot.layoutContainer);
                        MissionUIItem missionItem = missionItemInstance.GetComponent<MissionUIItem>();
                        missionItem.Init(scheduled.Mission, scrollViewContent.transform); // Oneline 
                    }
                }

                HangarSlot hangarSlot = HangarManager.GetSlotByNode(scheduleSlot.hangarNode); // Reorder
                if (HangarManager.ShipIsDockedAtNode(scheduleSlot.hangarNode))
                {
                    // Put a pilot in the slot if its ship has not been launched yet
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
