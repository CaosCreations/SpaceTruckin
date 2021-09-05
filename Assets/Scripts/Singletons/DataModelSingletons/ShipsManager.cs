using UnityEngine;

public class ShipsManager : MonoBehaviour, IDataModelManager
{
    public static ShipsManager Instance { get; private set; }

    public GameObject shipInstancePrefab;
    [SerializeField] private ShipsContainer shipsContainer;
    public Ship[] Ships => shipsContainer.Elements;

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
        if (DataUtils.SaveFolderExists(Ship.FolderName))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Ship.FolderName);
        }

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

    public static void RepairShip()
    {
        Ship ship = HangarManager.GetShipByNode(UIManager.HangarNode);

        if (ship != null)
        {
            RepairShip(ship);
        }
        else
        {
            Debug.LogError($"Cannot repair ship at node {UIManager.HangarNode} as the ship is null.");
        }
    }

    public static void RepairShip(Ship ship)
    {
        ship.CurrentHullIntegrity = Mathf.Min(
            ship.CurrentHullIntegrity + RepairsConstants.HullRepairedPerWin,
            ship.MaxHullIntegrity);
    }

    public static bool ShipIsLaunched(Ship ship)
    {
        ScheduledMission scheduled = MissionsManager.GetScheduledMission(ship);
        return scheduled?.Mission != null && scheduled.Mission.IsInProgress();
    }

    /// <summary>Warp is required to go out on missions</summary>
    public static void EnableWarp(Ship ship)
    {
        ship.CanWarp = true;
    }

    #region Persistence
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
        DataUtils.RecursivelyDeleteSaveData(Ship.FolderName);
    }
    #endregion
}
