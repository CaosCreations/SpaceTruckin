using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HangarEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Hangar/Dock Ship at First Available Node")]
    private static Ship DockShipAtAvailableNode()
    {
        try
        {
            HangarSlot hangarSlot = HangarManager.HangarSlots
                .FirstOrDefault(x => x.IsUnlocked && x.Ship == null);
            
            Ship ship = ShipsManager.Instance.Ships.FirstOrDefault(s => s.IsInQueue);
            
            HangarManager.DockShip(ship, hangarSlot.Node);

            return ship;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }

    [MenuItem("Space Truckin/Hangar/Dock Ship at First Available Node and Assign Random Mission")]
    private static void DockAndAssignMission()
    {
        try
        {
            Ship ship = DockShipAtAvailableNode();

            Mission mission = MissionsManager.Instance.Missions
                .FirstOrDefault(x => MissionsManager.GetScheduledMission(x) == null);

            MissionsManager.AddOrUpdateScheduledMission(ship.Pilot, mission);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Hangar/Launch All Ships On Missions")]
    private static void LaunchAllShips()
    {
        try
        {
            foreach (HangarSlot hangarSlot in HangarManager.HangarSlots)
            {
                if (hangarSlot.Ship == null)
                {
                    continue;
                }

                ScheduledMission scheduled = MissionsManager.GetScheduledMission(hangarSlot.Ship);
                scheduled.Mission.StartMission();

                MissionsManager.RemoveScheduledMission(scheduled);

                HangarManager.LaunchShip(hangarSlot.Node);
                hangarSlot.LaunchShip();
            }

            HangarNodeUI nodeUI = FindObjectOfType<HangarNodeUI>();
            nodeUI.shipToInspect = null;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
