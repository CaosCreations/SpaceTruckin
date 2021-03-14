using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HangarManager : MonoBehaviour, IDataModelManager
{
    public static HangarManager Instance { get; private set; }

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
        MissionUIItem.OnMissionUnscheduled += DestroyShipInstance;
    }

    public static void DockShip(Ship ship, Hangar hangar, int node /*HangarSlotNumber slotNumber*/)
    {
        if (ship != null && NodeIsValid(node))
        {
            HangarSlot targetSlot = hangar.HangarSlots.FirstOrDefault(x => x.Node == node);
            if (targetSlot != null && targetSlot.IsUnlocked)
            {
                targetSlot.Ship = ship;

                // Advance the queue once the ship enters the hangar 
                if (Instance.shipQueue.Contains(ship))
                {
                    Instance.shipQueue.Remove(ship);
                }

                if (targetSlot.ShipInstance != null)
                {
                    // Destroy the ship object if one already exists
                    Instance.DestroyShipInstance(targetSlot);
                }

                // Create a new ship object inside the target slot 
                Instance.InitShipInstance(ship, targetSlot);
            }
        }
        else
        {
            Debug.Log($"Could not dock ship at node {node}");
        }
    }

    public static void DockShip(Ship ship, int node)
    {
        GetSlotByNode(node).Ship = ship;
    }

    public void InitShipInstance(Ship ship, HangarSlot slot)
    {
        GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, slot.transform);
        Instantiate(ship.ShipPrefab, shipParentInstance.transform);
        ShipInstance instance = shipParentInstance.GetComponent<ShipInstance>();
        slot.ShipInstance = instance;
    }

    public void DestroyShipInstance(HangarSlot slot)
    {
        Destroy(slot.ShipInstance.transform.parent);
        Destroy(slot.ShipInstance);
    }

    public static bool NodeIsUnlocked(int node)
    {
        return node <= HangarConstants.StartingNumberOfSlots + LicencesManager.HangarSlotUnlockEffect; 
    }

    public static Ship GetShipByNode(int node)
    {
        // If using int nodes, then just search for that
        // If using 6 element enum, then do a 2D loop through hangars 

        return Instance.hangarSlots.FirstOrDefault(x => x.Node == node).Ship;
    }

    public static int GetNodeByShip(Ship ship)
    {
        // If using int nodes, then just search for that
        // If using 6 element enum, then do a 2D loop through hangars 

        return Instance.hangarSlots.FirstOrDefault(x => x.Ship == ship).Node;
    }

    public static HangarSlot GetSlotByNode(int node)
    {
        return Instance.hangarSlots.FirstOrDefault(x => x.Node == node);
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

    public static HangarSlot[] GetUnlockedHangarSlots()
    {
        return Instance.hangarSlots
            .Where(x => x.Node <= LicencesManager.HangarSlotUnlockEffect)
            .ToArray();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    #region Persistence
    public void SaveData()
    {
        if (hangarSlots != null)
        {
            string json = JsonUtility.ToJson(hangarSlots);
            DataUtils.SaveFileAsync(HangarSlot.FILE_NAME, HangarSlot.FOLDER_NAME, json);
        }
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(HangarSlot.FOLDER_NAME);
    }

    public void LoadDataAsync()
    {
        for (int i = 0; i < HangarConstants.TotalNumberOfSlots; i++)
        {
            //hangarSlots[i] = new HangarSlot();

        }

        //hangarSlots = DataUtils.LoadFileAsync<>
        //hangarSlots = DataUtils.LoadFileAsync<HangarSlot[]>(HangarSlot.FILE_NAME, HangarSlot.FOLDER_NAME);
    }
    #endregion
}
