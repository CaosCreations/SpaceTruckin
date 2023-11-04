using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class HangarEditor : MonoBehaviour
{
    [MenuItem("Space Truckin/Hangar/Dock Ship at First Available Node")]
    public static Ship DockShipAtAvailableNode(out int hangarNode)
    {
        hangarNode = -1;
        try
        {
            HangarSlot hangarSlot = HangarManager.HangarSlots.FirstOrDefault(x => x.IsUnlocked && x.Ship == null);

            hangarNode = hangarSlot.Node;

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
            // Ensure we have ships and missions available 
            PilotsEditor.HireAll();
            MissionsEditor.AcceptAll();

            Ship ship = DockShipAtAvailableNode(out int hangarNode);

            // Prepare ship for launch
            ShipsEditor.MaxOutResources();

            Mission mission = MissionsManager.Instance.Missions.FirstOrDefault(x => MissionsManager.GetScheduledMission(x) == null);

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
            HangarManager.LaunchAllShips();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    [MenuItem("Space Truckin/Hangar/Assign Missions to Ships at All Nodes and Launch All On Missions")]
    private static void AssignAndLaunchShipsAtEveryNode()
    {
        try
        {
            foreach (HangarSlot hangarSlot in HangarManager.HangarSlots)
            {
                DockAndAssignMission();
            }

            LaunchAllShips();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
