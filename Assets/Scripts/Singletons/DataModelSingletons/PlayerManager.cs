using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataModelManager
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    [SerializeField] private PlayerData playerData;
    public string PlayerName
    {
        get => playerData.PlayerName; set => playerData.PlayerName = value;
    }
    public long Money
    {
        get => playerData.PlayerMoney;
        set => playerData.PlayerMoney = value;
    }
    public long TotalMoneyAcquired
    {
        get => playerData.PlayerTotalMoneyAcquired; 
        set => playerData.PlayerTotalMoneyAcquired = value;
    }
    public int LicencePoints
    {
        get => playerData.PlayerLicencePoints;
    }
    public int TotalLicencePointsAcquired
    {
        get => playerData.PlayerTotalLicencePointsAcquired;
    }
    public int RepairTools 
    { 
        get => playerData.PlayerRepairTools; set => playerData.PlayerRepairTools = value; 
    }



    public static bool CanRepair => Instance.RepairTools > 0;
    public static bool IsPaused { get; set; }

    public static GameObject PlayerObject { get; private set; }
    public static PlayerMovement PlayerMovement { get; private set; }

    public static event System.Action OnFinancialTransaction;

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
        if (DataUtils.SaveFolderExists(PlayerData.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(PlayerData.FOLDER_NAME);
        }

        PlayerObject = GameObject.FindGameObjectWithTag(PlayerConstants.PlayerTag);
        if (PlayerObject != null)
        {
            PlayerMovement = PlayerObject.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player object not found");
        }

        if (playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    public bool CanSpendMoney(long amount)
    {
        if (amount <= Instance.Money)
        {
            return true;
        }
        return false;
    }

    public void SpendMoney(long amount)
    {
        Instance.Money -= amount;
        OnFinancialTransaction?.Invoke();
    }

    public void ReceiveMoney(long amount)
    {
        Instance.Money += amount;
        Instance.TotalMoneyAcquired += amount;
        OnFinancialTransaction?.Invoke();
    }

    public void AcquireLicence(Licence licence)
    {
        if (playerData.PlayerLicencePoints >= licence.PointsCost)
        {
            playerData.PlayerLicencePoints -= licence.PointsCost;
            playerData.PlayerTotalLicencePointsAcquired += licence.PointsCost;
            licence.IsOwned = true;
            Debug.Log($"{licence.Name} has been acquired\nRemaining LP: {playerData.PlayerLicencePoints}");
        }
        else
        {
            Debug.Log($"Player has insufficient LP to acquire {licence.Name}");
        }
    }

    public void EnterMenuState()
    {
        PlayerMovement.ResetDirection();
        IsPaused = true;
    }

    public static void SetPlayerName(string playerName)
    {
        Instance.PlayerName = playerName;
        Debug.Log($"Player name set to: {Instance.PlayerName}");
    }

    

    #region Persistence
    public void SaveData()
    {
        playerData.SaveData();
    }

    public async void LoadDataAsync()
    {
        await playerData.LoadDataAsync();
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FOLDER_NAME);
    }
    #endregion
}
