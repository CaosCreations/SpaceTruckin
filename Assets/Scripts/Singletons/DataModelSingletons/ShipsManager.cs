using UnityEngine;
using System.Linq;

public enum HangarNode
{
    None, One, Two, Three, Four, Five, Six
}

public class ShipsManager : MonoBehaviour, IDataModelManager
{
    public static ShipsManager Instance;

    public GameObject shipInstancePrefab;
    [SerializeField] private ShipsContainer shipsContainer;
    public Ship[] Ships { get => shipsContainer.ships; }

    public HangarSlot[] hangarSlots;

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

    public void Init()
    {
        if (DataModelsUtils.SaveFolderExists(Ship.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Ship.FOLDER_NAME);
        }
        hangarSlots = FindObjectsOfType<HangarSlot>();
        UpdateHangarShips();

        if (Ships == null)
        {
            Debug.LogError("No ship data");
        }
    }

    public static void DamageShip(Ship ship, int damage)
    {
        ship.CurrentHullIntegrity = Mathf.Max(
            0, ship.CurrentHullIntegrity - damage);
    }

    public static void LaunchShip(HangarNode node)
    {
        foreach (HangarSlot slot in Instance.hangarSlots)
        {
            if (slot.node == node)
            {
                Ship ship = GetShipForNode(node);
                if(ship != null)
                {
                    ship.CurrentMission.StartMission();
                    ship.IsLaunched = true;
                    slot.LaunchShip();
                }
            }
        }
    }

    public static Ship GetShipForNode(HangarNode node)
    {
        foreach(Ship ship in Instance.Ships)
        {
            if(ship.HangarNode == node)
            {
                return ship;
            }
        }

        return null;
    }

    public static void UpdateHangarShips()
    {
        ClearSlots();
        foreach (Ship ship in Instance.Ships)
        {
            if (ship.IsOwned && !ship.IsLaunched)
            {
                HangarSlot shipSlot = GetShipSlot(ship);

                if(shipSlot != null)
                {
                    GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, shipSlot.transform);
                    Instantiate(ship.ShipPrefab, shipParentInstance.transform);
                    ShipInstance instance = shipParentInstance.GetComponent<ShipInstance>();
                    shipSlot.shipInstance = instance;
                }
                else
                {
                    Debug.Log("Ship Hangar node not set");
                }
            }
        }
    }

    public static Ship NodeHasShip(HangarNode node)
    {
        Ship ship = Instance.Ships.Where(x => x.HangarNode == node).FirstOrDefault();
        if (ship != null)
        {
            return ship;
        }

        return null;
    }

    private static HangarSlot GetShipSlot(Ship ship)
    {
        foreach(HangarSlot slot in Instance.hangarSlots)
        {
            if(slot.node == ship.HangarNode)
            {
                return slot;
            }
        }

        return null;
    }

    private static void ClearSlots()
    {
        foreach (HangarSlot slot in Instance.hangarSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    public void SaveData()
    {
        foreach (Ship ship in Instance.Ships)
        {
            ship.SaveData();
        }
    }

    public async void LoadDataAsync()
    {
        foreach (Ship ship in Instance.Ships)
        {
            await ship.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        DataModelsUtils.RecursivelyDeleteSaveData(Ship.FOLDER_NAME);
    }
}
