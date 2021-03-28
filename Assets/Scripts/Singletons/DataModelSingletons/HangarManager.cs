﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HangarManager : MonoBehaviour
{
    public static HangarManager Instance { get; private set; }

    [SerializeField] private GameObject shipInstancePrefab;

    public static HangarSlot[] HangarSlots { get; private set; }
    public static GameObject BatteriesContainer { get; private set; }

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

        HangarSlots = FindObjectsOfType<HangarSlot>();
        if (HangarSlots == null)
        {
            Debug.LogError("Hangar slots not found");
        }

        BatteriesContainer = GameObject.FindGameObjectWithTag(
            HangarConstants.BatteriesContainerTag);
}

public static void DockShip(Ship ship, int node)
    {
        if (ship != null && NodeIsValid(node))
        {
            HangarSlot slot = GetSlotByNode(node);
            if (slot != null && slot.IsUnlocked)
            {
                slot.Ship = ship;

                // Create a new ship object inside the target slot 
                InitShipInstance(ship, slot);

                Debug.Log($"{ship} successfully docked at node {node}");
            }
        }
        else
        {
            Debug.LogError($"Could not dock ship at node {node}");
        }
    }

    public static void RequeueShip(int node)
    {
        Ship ship = GetShipByNode(node);
        HangarSlot slot = GetSlotByNode(node);

        if (ship != null && slot != null)
        {
            slot.RequeueShip();
            Debug.Log($"{ship} successfully launched from node {node}");
        }
        else
        {
            Debug.LogError($"Couldn't launch ship at node {node}");
        }
    }

    public static void InitShipInstance(Ship ship, HangarSlot slot)
    {
        GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, slot.transform);
        if (ship.ShipPrefab != null)
        {
            Instantiate(ship.ShipPrefab, shipParentInstance.transform);
        }
        ShipInstance instance = shipParentInstance.GetComponent<ShipInstance>();
        slot.ShipInstance = instance;
    }

    public static bool NodeIsUnlocked(int node)
    {
        return node <= HangarConstants.StartingNumberOfSlots + LicencesManager.HangarSlotUnlockEffect;
    }

    public static Pilot GetPilotByNode(int node)
    {
        HangarSlot slot = GetSlotByNode(node);
        if (slot != null && slot.Ship != null && slot.Ship.Pilot != null)
        {
            return slot.Ship.Pilot;
        }
        return null;
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

    public static HangarSlot GetSlotByNode(int node)
    {
        return HangarSlots.FirstOrDefault(x => x.Node == node);
    }

    public static HangarSlot GetSlotByShip(Ship ship)
    {
        foreach (HangarSlot slot in HangarSlots)
        {
            if (slot != null
                && slot.Ship != null
                && slot.Ship == ship)
            {
                return slot;
            }
        }
        return null;
    }

    public static bool ShipIsDocked(Ship ship)
    {
        return GetSlotByShip(ship) != null;
    }

    public static bool ShipIsDocked(int node)
    {
        HangarSlot slot = GetSlotByNode(node);
        return slot != null && slot.Ship != null;
    }

    public static bool ShipIsDocked(HangarSlot slot)
    {
        return slot != null && slot.Ship != null;
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
}
