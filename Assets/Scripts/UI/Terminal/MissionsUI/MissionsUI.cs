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
            if(slot.missionLayoutContainer.childCount > 0)
            {
                Destroy(slot.missionLayoutContainer.GetChild(0).gameObject);
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
        //foreach (Transform child in scrollViewContent.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach (Mission mission in MissionsManager.GetAcceptedMissions())
        {
            GameObject scrollItem = Instantiate(missionItemPrefab, scrollViewContent.transform);
            MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
            missionItem.Init(mission, scrollViewContent.transform);
        }
        
        // Todo: only do this on enable 
        List<ScheduledMission> scheduledMissions = MissionsManager.GetScheduledMissionsStillInHangar();
        foreach (MissionScheduleSlot slot in missionSlots)
        {
            //Mission missionForSlot = scheduledMissions.Where(x => x.Pilot == slot.Pilot).FirstOrDefault();
            ScheduledMission missionInSlot = scheduledMissions.FirstOrDefault(x => x == slot.ScheduledMission);

            if (missionInSlot != null)
            {
                GameObject scrollItem = Instantiate(missionItemPrefab, slot.missionLayoutContainer);
                MissionUIItem missionItem = scrollItem.GetComponent<MissionUIItem>();
                missionItem.Init(missionInSlot.mission, scrollViewContent.transform);
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

    public void PopulatePilotSelect(MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        scrollViewContent.transform.DestroyDirectChildren();
        Pilot[] pilots = PilotsManager.GetPilotsAvailableForMissions();
        foreach (Pilot pilot in pilots)
        {
            if (pilot != null)
            {
                GameObject pilotSelectItem = Instantiate(pilotSelectItemPrefab, scrollViewContent.transform);
                pilotSelectItem.GetComponent<PilotSelectItem>().Init(pilot, scheduleSlot, mission);
            }
        }
    }
}
