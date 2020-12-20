using UnityEngine;
using System.Linq;

public enum HangarNode
{
    None, One, Two, Three, Four, Five, Six
}

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager Instance;

    public GameObject shipInstancePrefab;
    public Ship[] ships;

    public HangarSlot[] hangarSlots;

    void Awake()
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
        Init();
    }

    public void Init()
    {
        hangarSlots = FindObjectsOfType<HangarSlot>();
        UpdateHangarShips();
    }

    public static void DamageShip(int index, int damage)
    {
        for (int i = 0; i < Instance.ships.Length; i++)
        {
            if (Instance.ships[i] != null && index == Instance.ships[i].id)
            {
                Instance.ships[i].currenthullIntegrity = Mathf.Max(0, Instance.ships[i].currenthullIntegrity - damage);
            }
        }
    }

    public static void LaunchShip(HangarNode node)
    {
        foreach (HangarSlot slot in Instance.hangarSlots)
        {
            if (slot.node == node)
            {
                GetShipForNode(node).hangarNode = HangarNode.None;
                slot.LaunchShip();
            }
        }
    }

    public static Ship GetShipForNode(HangarNode node)
    {
        foreach(Ship ship in Instance.ships)
        {
            if(ship.hangarNode == node)
            {
                return ship;
            }
        }

        return null;
    }

    public static void UpdateHangarShips()
    {
        ClearSlots();
        foreach (Ship ship in Instance.ships)
        {
            if (ship.isOwned)
            {
                HangarSlot shipSlot = GetShipSlot(ship);

                if(shipSlot != null)
                {
                    GameObject shipParentInstance = Instantiate(Instance.shipInstancePrefab, shipSlot.transform);
                    Instantiate(ship.shipPrefab, shipParentInstance.transform);
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

    public static bool NodeHasShip(HangarNode node)
    {
        Ship ship = Instance.ships.Where(x => x.hangarNode == node).FirstOrDefault();
        if (ship != null)
        {
            return true;
        }

        return false;
    }

    private static HangarSlot GetShipSlot(Ship ship)
    {
        foreach(HangarSlot slot in Instance.hangarSlots)
        {
            if(slot.node == ship.hangarNode)
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
}
