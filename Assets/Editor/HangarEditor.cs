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
            
            Ship ship = default;

            if (hangarSlot != null)
            {
                ship = ShipsManager.Instance.Ships.FirstOrDefault(s => s.IsInQueue);

                HangarManager.DockShip(ship, hangarSlot.Node);
            }

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
            
            if (mission != null)
            {
                MissionsManager.AddOrUpdateScheduledMission(ship.Pilot, mission);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
