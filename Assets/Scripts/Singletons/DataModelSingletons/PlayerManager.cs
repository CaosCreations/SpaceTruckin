using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDataModelManager, ILuaFunctionRegistrar
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    [SerializeField] private PlayerData playerData;

    public static event System.Action OnFinancialTransaction;

    #region Property Accessors
    public string PlayerName
    {
        get => playerData.PlayerName; set => playerData.PlayerName = value;
    }
    public string SpriteName
    {
        get => playerData.SpriteName; set => playerData.SpriteName = value;
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
    public int LicencePoints => playerData.PlayerLicencePoints;
    public int TotalLicencePointsAcquired => playerData.PlayerTotalLicencePointsAcquired;

    public int RepairTools
    {
        get => playerData.PlayerRepairTools; set => playerData.PlayerRepairTools = value;
    }

    public static bool CanRepair => Instance.RepairTools > 0;
    public static bool IsPaused { get; set; }

    public static GameObject PlayerObject { get; private set; }
    public static PlayerMovement PlayerMovement { get; private set; }
    public GameObject PlayerPrefab { get => playerData.PlayerPrefab; set => playerData.PlayerPrefab = value; }
    public Vector3 StationSpawnPosition => playerData.StationSpawnPosition;

    [SerializeField]
    private Transform playerParentTransform;
    #endregion

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

        // Todo: Temporary until we move singleton manager manager out of station scene
        Init();
    }

    public void Init()
    {
        if (DataUtils.SaveFolderExists(PlayerData.FolderName))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(PlayerData.FolderName);
        }

        //FindSceneObjects();
        RegisterLuaFunctions();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        if (DialogueManager.Instance != null)
        {
            // Pause when a conversation starts and unpause when it ends
            DialogueManager.Instance.conversationStarted += (t) =>
            {
                IsPaused = true;
                PlayerAnimationManager.ResetBoolParameters();
            };

            DialogueManager.Instance.conversationEnded += (t) => IsPaused = false;
        }

        // Init prefab when scene loads 
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            InstantiatePlayer();
        };
    }

    private void InstantiatePlayer()
    {
        if (PlayerPrefab == null)
        {
            throw new System.Exception("Player prefab was null. Cannot instantiate player.");
        }

        PlayerObject = Instantiate(PlayerPrefab, playerParentTransform);
        PlayerObject.transform.position = StationSpawnPosition;
        PlayerMovement = PlayerObject.GetComponent<PlayerMovement>();
    }

    private static void FindSceneObjects()
    {
        PlayerObject = GameObject.FindGameObjectWithTag(PlayerConstants.PlayerTag);

        if (PlayerObject != null)
        {
            PlayerMovement = PlayerObject.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player object not found");
        }

        if (Instance.playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    private void OnDisable() => UnregisterLuaFunctions();

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

    public void SpendMoney(double amount)
    {
        Instance.Money -= (long)amount;
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

    public static void EnterPausedState()
    {
        if (PlayerMovement != null)
            PlayerMovement.ResetDirection();

        IsPaused = true;
    }

    public static void EnterPausedState(PlayableDirector playableDirector)
    {
        PlayerMovement.ResetDirection();
        IsPaused = true;
    }

    public string GetPlayerName()
    {
        return PlayerName;
    }

    public static void SetPlayerName(string playerName)
    {
        Instance.PlayerName = playerName;
        Debug.Log($"Player name set to: {Instance.PlayerName}");
    }

    public static void SetSpriteName(string spriteName)
    {
        Instance.SpriteName = spriteName;
        Debug.Log($"Player name set to: {Instance.SpriteName}");
    }

    #region Lua Function Registration
    public void RegisterLuaFunctions()
    {
        if (DialogueManager.Instance == null)
            return;

        Lua.RegisterFunction(
            DialogueConstants.SpendMoneyFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => SpendMoney(0D)));

        Lua.RegisterFunction(
            DialogueConstants.GetPlayerNameFunctionName,
            this,
            SymbolExtensions.GetMethodInfo(() => GetPlayerName()));
    }

    public void UnregisterLuaFunctions()
    {
        Lua.UnregisterFunction(DialogueConstants.SpendMoneyFunctionName);

        Lua.UnregisterFunction(DialogueConstants.GetPlayerNameFunctionName);
    }
    #endregion

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
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FolderName);
    }
    #endregion
}
