using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataModelManager
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    [SerializeField] private PlayerData playerData;
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

    [Header("Set at Runtime")]
    public bool isPaused;
    public PlayerMovement playerMovement;

    public static event System.Action onFinancialTransaction;

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
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    public bool CanSpendMoney(long amount)
    {
        if (amount < Instance.Money)
        {
            return true;
        }
        return false;
    }

    public void SpendMoney(long amount)
    {
        Instance.Money -= amount;
        onFinancialTransaction?.Invoke();
    }

    public void ReceiveMoney(long amount)
    {
        Instance.Money += amount;
        Instance.TotalMoneyAcquired += amount;
        onFinancialTransaction?.Invoke();
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
        playerMovement.ResetAnimator();
        Instance.isPaused = true;
    }

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
}
