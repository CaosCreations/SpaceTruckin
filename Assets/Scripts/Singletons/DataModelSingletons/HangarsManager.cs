using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HangarsManager : MonoBehaviour, IDataModelManager
{
    public static HangarsManager Instance { get; private set; }

    public Hangar[] hangars;
    public HangarSlot[] hangarSlots; // Use this if doing n int nodes (not enum)
    public List<Ship> shipQueue;

    public GameObject shipInstancePrefab;

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
    }

    public void DockShip(Ship ship, Hangar hangar, int node /*HangarSlotNumber slotNumber*/)
    {
        // Use int param to index array 
        // Or lookup the value 

        //hangar.HangarSlots[slotNumber].ship = ship;

        if (ship != null && NodeIsValid(node))
        {
            HangarSlot targetSlot = hangar.HangarSlots.FirstOrDefault(x => x.node == node);
            if (targetSlot != null && targetSlot.IsUnlocked)
            {
                targetSlot.ship = ship;

                // Advance the queue once ship enters the hangar 
                if (shipQueue.Contains(ship))
                {
                    shipQueue.Remove(ship);
                }

                // Create the ship object inside the target slot 
                InitShipInstance(ship, targetSlot);
            }
        }
        else
        {
            Debug.Log($"Could not dock ship at node {node}");
        }
    }

    public void InitShipInstance(Ship ship, HangarSlot slot)
    {
        GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, slot.transform);
        Instantiate(ship.ShipPrefab, shipParentInstance.transform);
        ShipInstance instance = shipParentInstance.GetComponent<ShipInstance>();
        slot.shipInstance = instance;
    }

    public static Ship GetShipByNode(int node)
    {
        // If using int nodes, then just search for that
        // If using 6 element enum, then do a 2D loop through hangars 

        return Instance.hangarSlots.FirstOrDefault(x => x.node == node).ship;
    }

    public static int GetNodeByShip(Ship ship)
    {
        // If using int nodes, then just search for that
        // If using 6 element enum, then do a 2D loop through hangars 

        return Instance.hangarSlots.FirstOrDefault(x => x.ship == ship).node;
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

    private static bool NodeIsValid(int node)
    {
        return node >= 1 && node <= HangarConstants.TotalNumberOfSlots;
    }
    
    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public void LoadDataAsync()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}