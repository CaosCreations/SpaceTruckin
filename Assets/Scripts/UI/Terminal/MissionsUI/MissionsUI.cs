using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsUI : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject missionItemPrefab;
    public GameObject pilotSelectItemPrefab;
    private MissionDetailsUI missionDetailsUI;

    public MissionScheduleSlot[] missionSlots;

    private void Start()
    {
        missionDetailsUI = GetComponent<MissionDetailsUI>();
    }

    private void OnEnable()
    {
        SetupMissionSlots();
        CleanMenu();
        PopulateMissionSelect();
    }

    private void SetupMissionSlots()
    {
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            // Missions can be dropped into the slot if the corresponding
            // node is unlocked AND there is no ship already at that node 
            slot.IsActive = HangarManager.NodeIsUnlocked(slot.hangarNode)
                && HangarManager.GetShipByNode(slot.hangarNode) == null;
        }
    }

    private void CleanMenu()
    {
        scrollViewContent.transform.DestroyDirectChildren();

        foreach (MissionScheduleSlot slot in missionSlots)
        {
            if(slot.slotTransform.childCount > 0)
            {
                Destroy(slot.slotTransform.GetChild(0).gameObject);
            }
        }

        if (missionDetailsUI != null)
        {
            missionDetailsUI.DestroyMissionDetails();
        }
    }

    private void PopulateMissionSelect()
    {
        foreach (Mission mission in MissionsManager.GetAcceptedMissions())
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission, scrollViewContent.transform);
        }

        List<Mission> scheduledMissions = MissionsManager.GetScheduledMissions();
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            Mission missionForSlot = scheduledMissions.Where(x => x.Pilot == slot.Pilot).FirstOrDefault();

            if (missionForSlot != null)
            {
                GameObject scrollItem = Instantiate(missionItemPrefab, slot.slotTransform);
                MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                missionItem.Init(missionForSlot, scrollViewContent.transform);
            }
        }
    }

    private void ShowMissionProgress(MissionUIItem missionItem)
    {
        string substring = missionItem.mission.DaysLeftToComplete > 1 ? "days" : "day";

        missionItem.missionNameText.SetText(missionItem.mission.Name 
            + $"\n({missionItem.mission.DaysLeftToComplete} {substring} remaining)", FontType.ListItem);
    }

    public MissionScheduleSlot GetSlotForMissionDrag(Vector2 position)
    {
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(slot.parentTransform, position)
                && slot.IsActive)
            {
                return slot;
            }
        }
        return null;
    }

    public void PopulatePilotSelect(Mission mission, MissionScheduleSlot scheduleSlot)
    {
        scrollViewContent.transform.DestroyDirectChildren();
        foreach (Pilot pilot in PilotsManager.GetPilotsAvailableForMissions())
        {
            GameObject pilotSelectItem = Instantiate(pilotSelectItemPrefab, scrollViewContent.transform);

            pilotSelectItem.GetComponent<PilotSelectItem>().Init(pilot, () =>
            {
                pilotSelectItem.transform.SetParent(scheduleSlot.transform);
                SelectPilot(pilot, mission, scheduleSlot.hangarNode);
            });
        }
    }

    private void SelectPilot(Pilot pilot, Mission mission, int hangarNode)
    {
        PilotsManager.AssignMissionToPilot(pilot, mission);
        HangarManager.DockShip(pilot.Ship, hangarNode);
        
        // Return to the mission select after a pilot has been selected
        scrollViewContent.transform.DestroyDirectChildren();
        PopulateMissionSelect();
    }
}
