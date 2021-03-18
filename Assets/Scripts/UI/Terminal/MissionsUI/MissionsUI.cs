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
        //CleanMenu();
        PopulateScheduleSlots();
        PopulateMissionSelect();
    }

    private void SetActiveSlots()
    {
        foreach (MissionScheduleSlot slot in scheduleSlots)
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
        foreach (Pilot pilot in pilotsToSelect)
        {
            if (pilot != null)
            {
                GameObject pilotSelectItem = Instantiate(pilotSelectItemPrefab, scrollViewContent.transform);
                pilotSelectItem.GetComponent<PilotSelectItem>().Init(pilot, scheduleSlot, mission);
            }
        }
    }

    private void PopulateScheduleSlots()
    {
        List<ScheduledMission> scheduledMissions = MissionsManager.GetScheduledMissionsStillInHangar();
        foreach (MissionScheduleSlot slot in scheduleSlots)
        {
            ScheduledMission scheduled = scheduledMissions.FirstOrDefault(x => x == slot.ScheduledMission);
            if (scheduled != null)
            {
                if (scheduled.mission != null)
                {
                    GameObject missionItemInstance = Instantiate(missionItemPrefab, slot.layoutContainer);
                    MissionUIItem missionItem = missionItemInstance.GetComponent<MissionUIItem>();
                    missionItem.Init(scheduled.mission, scrollViewContent.transform);
                }

                if (scheduled.pilot != null)
                {
                    slot.PutPilotInSlot(scheduled.pilot);
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
