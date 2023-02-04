using UnityEngine;

public class ShipsManager : MonoBehaviour, IDataModelManager
{
    public static ShipsManager Instance { get; private set; }

    public GameObject shipInstancePrefab;
    [SerializeField] private ShipsContainer shipsContainer;
    public Ship[] Ships => shipsContainer.Elements;

    public static ShipUnderRepair ShipUnderRepair = new ShipUnderRepair();
    public static bool CanRepair => PlayerManager.CanRepair
        && !ShipUnderRepair.IsFullyRepaired;

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
        if (Ships == null)
        {
            Debug.LogError("No ship data");
        }

        HangarNodeUI.OnHangarNodeTerminalOpened += SetupShipUnderRepair;
        HangarNodeUI.OnHangarNodeTerminalClosed += ResetShipUnderRepair;
    }

    public static void DamageShip(Ship ship, int damage)
    {
        if (ship != null)
        {
            ship.CurrentHullIntegrity = Mathf.Max(0, ship.CurrentHullIntegrity - damage);
        }
    }

    public static void RepairShip()
    {
        if (ShipUnderRepair.Ship != null)
        {
            RepairShip(ShipUnderRepair.Ship);
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

    private static void SetupShipUnderRepair(Ship ship)
    {
        ShipUnderRepair.Ship = ship;

        if (ArchivedMissionsManager.Instance != null)
        {
            var mostRecentMission = ArchivedMissionsManager.GetMostRecentMissionByPilot(ShipUnderRepair.Pilot);

            if (mostRecentMission != null)
            {
                ShipUnderRepair.DamageType = mostRecentMission.DamageType;
            }
        }
    }

    private static void ResetShipUnderRepair()
    {
        ShipUnderRepair.Reset();
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
        if (!DataUtils.SaveFolderExists(Ship.FolderName))
        {
            DataUtils.CreateSaveFolder(Ship.FolderName);
            return;
        }

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
