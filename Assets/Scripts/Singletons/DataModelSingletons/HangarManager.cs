using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HangarManager : MonoBehaviour
{
    public static HangarManager Instance { get; private set; }

    public HangarSlot[] hangarSlots;
    public List<Ship> shipQueue;
    public GameObject shipInstancePrefab;
    private MissionScheduleSlot[] scheduleSlots;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
         
            return;
        }

        hangarSlots = FindObjectsOfType<HangarSlot>();
        if (hangarSlots == null)
        {
            Debug.LogError("Hangar slots not found");
        }

        scheduleSlots = FindObjectsOfType<MissionScheduleSlot>();
        if (scheduleSlots == null)
        {
            Debug.LogError("Schedule slots not found");
        }
    }

    public static void DockShip(Ship ship, int node)
    {
        if (ship != null && NodeIsValid(node))
        {
            HangarSlot slot = GetSlotByNode(node);
            if (slot != null && slot.IsUnlocked)
            {
                slot.Ship = ship;

                // Update the queue once the ship enters the hangar 
                if (Instance.shipQueue.Contains(ship))
                {
                    Instance.shipQueue.Remove(ship);
                }

                if (slot.ShipInstance != null)
                {
                    // Destroy the ship object if one already exists
                    Instance.DestroyShipInstance(slot);
                }

                // Create a new ship object inside the target slot 
                Instance.InitShipInstance(ship, slot);
            }
        }
        else
        {
            Debug.Log($"Could not dock ship at node {node}");
        }
    }

    public static void LaunchShip(int node)
    {
        Ship ship = GetShipByNode(node);
        HangarSlot slot = GetSlotByNode(node);

        ScheduledMission scheduled = MissionsManager.GetScheduledMission(ship);
        if (scheduled != null)
        {
            ship.CurrentMission.StartMission();
        }
        ship.IsLaunched = true; // Can probably get rid of this flag 
        slot.LaunchShip();

        // Open up the schedule slot for the next ship
        MissionScheduleSlot scheduleSlot = GetScheduleSlotByNode(node);
        scheduleSlot.IsActive = true;
    }

    public void InitShipInstance(Ship ship, HangarSlot slot)
    {
        GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, slot.transform);
        if (ship.ShipPrefab != null)
        {
            Instantiate(ship.ShipPrefab, shipParentInstance.transform);
        }
        ShipInstance instance = shipParentInstance.GetComponent<ShipInstance>();
        slot.ShipInstance = instance;
    }

    public void DestroyShipInstance(HangarSlot slot)
    {
        //Destroy(slot.ShipInstance);
        
        if (slot.ShipInstance != null)
        {
            Destroy(slot.ShipInstance.transform.parent);
        }
    }

    public static bool NodeIsUnlocked(int node)
    {
        return node <= HangarConstants.StartingNumberOfSlots + LicencesManager.HangarSlotUnlockEffect; 
    }

    public static Ship GetShipByNode(int node)
    {
        HangarSlot slot = GetSlotByNode(node);
        if (slot != null && slot.Ship != null)
        {
            return slot.Ship;
        }
        return null;
    }

    public static int GetNodeByShip(Ship ship)
    {
        return Instance.hangarSlots.FirstOrDefault(x => x.Ship == ship).Node;
    }

    public static HangarSlot GetSlotByNode(int node)
    {
        return Instance.hangarSlots.FirstOrDefault(x => x.Node == node);
    }

    public static HangarSlot GetSlotByPilot(Pilot pilot)
    {
        foreach (HangarSlot slot in Instance.hangarSlots)
        {
            if (slot.Ship == null || slot.Ship.Pilot == null)
            {
                continue;
            }
            if (slot.Ship.Pilot == pilot)
            {
                return slot;
            }
        }
        return null;
    }

    public static bool ShipIsDocked(Ship ship)
    {
        return GetNodeByShip(ship) != -1;
    }

    // Call this at the end of the day to get the total queue for the next day
    public List<Ship> GetShipsInQueue()
    {
        return ShipsManager.Instance.Ships.Where(x => x.IsInQueue).ToList();
    }

    public static bool NodeIsValid(int node)
    {
        return node >= 1 && node <= HangarConstants.MaximumNumberOfSlots;
    }

    public static HangarSlot[] GetUnlockedHangarSlots()
    {
        return Instance.hangarSlots
            .Where(x => x.Node <= LicencesManager.HangarSlotUnlockEffect)
            .ToArray();
    }

    public static MissionScheduleSlot GetScheduleSlotByNode(int node)
    {
        return Instance.scheduleSlots.FirstOrDefault(x => x.hangarNode == node);
    }
}
