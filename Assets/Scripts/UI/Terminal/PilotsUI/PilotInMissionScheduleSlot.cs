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

    /// <summary>Either remove or choose a new pilot based on circumstances</summary>
    /// <param name="isReplacingPilot">When left clicking, this will be true</param>
    private void RemoveOrReplacePilot(bool isReplacingPilot)
    {
        if (pilot != null && scheduleSlot != null)
        {
            ScheduledMission scheduled = MissionsManager.GetScheduledMission(pilot);
            Mission mission = scheduled?.Mission;
            if (mission != null)
            {
                if (isReplacingPilot)
                {
                    // Give the option to replace the pilot if we change our mind.
                    // We don't send them back to the queue immediately in case we want to revert.
                    missionsUI.PopulatePilotSelect(scheduleSlot, mission);
                    return;
                }

                // We are removing the new pilot,
                // so we unschedule the mission as it's invalid without a pilot.
                MissionsManager.RemoveScheduledMission(scheduled);
            }

            // Ship goes back into the queue as it's no longer needed.
            HangarManager.LaunchShip(scheduleSlot.hangarNode);

            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Left click to replace 
            RemoveOrReplacePilot(isReplacingPilot: true);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Right click to remove 
            RemoveOrReplacePilot(isReplacingPilot: false);
        }
    }
}
