﻿using Events;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PilotSelectItem : MonoBehaviour
{
    private Pilot pilot;
    private Mission mission;
    private MissionsUI missionsUI;
    private MissionScheduleSlot scheduleSlot;
    [SerializeField] private Button itemButton;
    [SerializeField] private Text itemText;
    [SerializeField] private Image itemImage;

    private void Start()
    {
        missionsUI = GetComponentInParent<MissionsUI>();
    }

    public void Init(Pilot pilot, MissionScheduleSlot scheduleSlot, Mission mission = null)
    {
        this.pilot = pilot;
        this.scheduleSlot = scheduleSlot;
        this.mission = mission;
        itemButton.AddOnClick(SelectPilotForSlot);
        itemText.SetText(BuildItemString());
        itemImage.sprite = pilot.Avatar;
    }

    public void SelectPilotForSlot()
    {
        if (pilot != null)
        {
            if (HangarManager.ShipIsDocked(scheduleSlot.hangarNode))
            {
                // Launch the ship to make room for the new one  
                HangarManager.LaunchShip(scheduleSlot.hangarNode);
            }

            // Dock the pilot's ship regardless of whether there's a mission in the slot
            HangarManager.DockShip(pilot.Ship, scheduleSlot.hangarNode);
            scheduleSlot.PutPilotInSlot(pilot);
            
            SingletonManager.EventService.Dispatch<OnPilotSlottedEvent>();
        }

        if (mission != null)
        {
            MissionsManager.AddOrUpdateScheduledMission(pilot, mission);
        }

        // Return to the mission select after a pilot has been selected
        missionsUI.PopulateMissionSelect();
        missionsUI.pilotSelectCloseButton.SetActive(false);
    }

    private string BuildItemString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLineWithBreaks(pilot.Name);
        builder.AppendLineWithBreaks(pilot.Ship.Name);
        builder.AppendLineWithBreaks($"Lv. {pilot.Level}");
        return builder.ToString();
    }
}
