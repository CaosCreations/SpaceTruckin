using UnityEngine;
using System.Linq;

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
        if (DataUtils.SaveFolderExists(Ship.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Ship.FOLDER_NAME);
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
        if (ship != null)
        {
            ship.CurrentHullIntegrity = Mathf.Max(
                0, ship.CurrentHullIntegrity - damage);
        }
    }

    public static void LaunchShip(int node)
    {
        foreach (HangarSlot slot in Instance.hangarSlots)
        {
            if (slot.Node == node)
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

    // To Update // - don't deduct fuel when assigning mission
    // its done before NDR 
    public static void DockShip(Ship ship)   
    {
        ship.DeductFuel();
        ship.IsLaunched = false;
        ship.CurrentMission.Ship = null;
        ship.CurrentMission = null;
    }

    public static void DockShip(Ship ship, HangarSlot hangarSlot)
    {
        hangarSlot.Ship = ship;
        // More?
    }

    public static Ship GetShipForNode(int node)
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
                    shipSlot.ShipInstance = instance;
                }
                else
                {
                    Debug.Log("Ship Hangar node not set");
                }
            }
        }
    }

    public static Ship NodeHasShip(int node)
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
            if(slot.Node == ship.HangarNode)
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
        DataUtils.RecursivelyDeleteSaveData(Ship.FOLDER_NAME);
    }
}
