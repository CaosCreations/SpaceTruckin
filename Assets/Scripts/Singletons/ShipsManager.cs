using UnityEngine;

public enum HangarNode
{
    One, Two, Three, Four, Five, Six, None
}

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager Instance;

    public Ship[] ships;

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
        foreach (Ship ship in Instance.ships)
        {
            if (ship.hangarNode == node)
            {
                ship.shipInstanceInHangar.Launch();
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
        foreach (Ship ship in Instance.ships)
        {
            if (ship.isOwned)
            {
                GameObject shipInstance = Instantiate(ship.shipPrefab);
                ship.shipInstanceInHangar = shipInstance.AddComponent<ShipInstance>();
            }
        }
    }
}
