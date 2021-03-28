using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilotInMissionScheduleSlot : MonoBehaviour, IPointerClickHandler
{
    public Pilot pilot;
    private MissionScheduleSlot scheduleSlot;
    private MissionsUI missionsUI;

    public void Init(Pilot pilot)
    {
        this.pilot = pilot;
        scheduleSlot = GetComponentInParent<MissionScheduleSlot>();
        missionsUI = GetComponentInParent<MissionsUI>();

        Image image = GetComponent<Image>();
        if (pilot != null && image != null)
        {
            image.sprite = pilot.Avatar;
        }
    }

    private void RemoveOrReplacePilot()
    {
        if (pilot != null && scheduleSlot != null)
        {
            ScheduledMission scheduled = MissionsManager.GetScheduledMission(pilot);
            Mission mission = scheduled?.Mission;
            if (mission != null)
            {
                // Give the option to replace the pilot if there was a mission in the slot
                missionsUI.PopulatePilotSelect(scheduleSlot, mission);
            }
            else
            {
                // The pilot has been removed, so we unschedule the mission
                MissionsManager.RemoveScheduledMission(scheduled);
                
                // Ship goes back into the queue 
                HangarManager.LaunchShip(scheduleSlot.hangarNode);

                Destroy(gameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RemoveOrReplacePilot();
        }
    }
}
