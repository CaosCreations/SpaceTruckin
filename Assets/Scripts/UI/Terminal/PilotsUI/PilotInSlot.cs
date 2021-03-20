using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotInSlot : MonoBehaviour, IPointerClickHandler
{
    public Pilot pilot;
    private MissionScheduleSlot scheduleSlot;

    public void Init(Pilot pilot)
    {
        this.pilot = pilot;
        scheduleSlot = GetComponentInParent<MissionScheduleSlot>();

        Image image = GetComponent<Image>();
        if (pilot != null && image != null)
        {
            image.sprite = pilot.Avatar;
        }
    }

    private void RemovePilot()
    {
        if (pilot != null)
        {
            MissionsManager.RemoveScheduledMission(pilot);
            if (scheduleSlot != null)
            {
                // Todo: Check if its actually there first 
                HangarManager.LaunchShip(scheduleSlot.hangarNode);

                // Reopen the schedule slot
                scheduleSlot.IsActive = true;
            }
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RemovePilot();
            
            MissionScheduleSlot scheduleSlot = GetComponentInParent<MissionScheduleSlot>();
            if (scheduleSlot != null)
            {
                scheduleSlot.IsActive = true;
            }

            // Remove the mission as it's redundant without a pilot
            MissionUIItem missionInSlot = transform.parent.GetComponentInChildren<MissionUIItem>();
            if (missionInSlot != null)
            {
                missionInSlot.Unschedule();
            }
        }
    }
}
