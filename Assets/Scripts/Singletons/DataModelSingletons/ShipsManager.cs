using UnityEngine;

public class ShipsManager : MonoBehaviour, IDataModelManager
{
    public static ShipsManager Instance;

    public GameObject shipInstancePrefab;
    [SerializeField] private ShipsContainer shipsContainer;
    public Ship[] Ships { get => shipsContainer.ships; }

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

    public static bool ShipIsLaunched(Ship ship)
    {
        ScheduledMission scheduled = MissionsManager.GetScheduledMission(ship);
        return scheduled?.Mission != null && scheduled.Mission.IsInProgress();
    }

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
        DataUtils.RecursivelyDeleteSaveData(Ship.FOLDER_NAME);
    }
    #endregion
}
