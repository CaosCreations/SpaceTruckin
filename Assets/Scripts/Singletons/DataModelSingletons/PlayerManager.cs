using Events;
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
    public AnimatorSettings PlayerAnimatorSettings
    {
        get => playerData.AnimatorSettings; set => playerData.AnimatorSettings = value;
    }
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

        RegisterSceneChangeEvents();
    }

    public void Init()
    {
        RegisterDialogueEvents();

#if UNITY_EDITOR
        // If starting from MainStation scene in the editor, then perform the setup here
        if (SceneLoadingManager.GetCurrentSceneType() == SceneType.MainStation)
        {
            SetUpPlayer();
        }
#endif
    }

    private void RegisterDialogueEvents()
    {
        SingletonManager.EventService.Add<OnConversationStartedEvent>(OnConversationStartedHandler);
        SingletonManager.EventService.Add<OnConversationEndedEvent>(OnConversationEndedHandler);
    }

    private void OnConversationStartedHandler(OnConversationStartedEvent evt)
    {
        EnterPausedState();
        PlayerAnimationManager.ResetBoolParameters();
    }

    private void OnConversationEndedHandler(OnConversationEndedEvent evt)
    {
        ExitPausedState();
    }

    private void RegisterSceneChangeEvents()
    {
        // Set up player when station scene loads 
        SceneManager.activeSceneChanged += (Scene previous, Scene next) =>
        {
            if (SceneLoadingManager.GetSceneNameByType(SceneType.MainStation) == next.name)
            {
                SetUpPlayer();
            }
        };
    }

    public void SetUpPlayer()
    {
        PlayerObject = GameObject.FindGameObjectWithTag(PlayerConstants.PlayerTag);

        if (PlayerObject == null)
        {
            throw new System.Exception("Player object not found");
        }

        if (Instance.playerData == null)
        {
            Debug.LogError("No player data found");
        }

        PlayerMovement = PlayerObject.GetComponent<PlayerMovement>();

        // Animator field setup 
        var playerAnimator = PlayerObject.GetComponent<Animator>();
        var animatorSettingsMapper = new PlayerAnimatorSettingsMapper();
        animatorSettingsMapper.MapSettings(playerAnimator, PlayerAnimatorSettings);

        SetStartingDataValues();
    }

    private static void SetStartingDataValues()
    {
        Instance.playerData.SetStartingValues();
    }

    public bool CanSpendMoney(long amount)
    {
        return amount <= Instance.Money;
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

    public void BuyRepairTools(int amount)
    {
        if (Instance.CanSpendMoney(amount * RepairsConstants.CostPerTool))
        {
            Instance.SpendMoney(amount * RepairsConstants.CostPerTool);
            Instance.RepairTools += amount;
            SingletonManager.EventService.Dispatch<OnRepairsToolBoughtEvent>();
        }
    }

    public void SpendRepairTool()
    {
        Instance.RepairTools = Mathf.Max(0, Instance.RepairTools - 1);
        SingletonManager.EventService.Dispatch<OnRepairsToolSpentEvent>();
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

    public static void EnterPausedState(bool stopClock = true)
    {
        if (PlayerMovement != null)
            PlayerMovement.MovementAnimation.ResetDirection();

        IsPaused = true;
        SingletonManager.EventService.Dispatch(new OnPlayerPausedEvent(stopClock));
    }

    public static void EnterPausedState(PlayableDirector playableDirector)
    {
        Debug.Log("Entering paused state from playable director: " + playableDirector.name);
        EnterPausedState();
    }

    public static void ExitPausedState()
    {
        IsPaused = false;
        SingletonManager.EventService.Dispatch<OnPlayerUnpausedEvent>();
    }

    public static void SetSpriteRendererEnabled(bool enabled)
    {
        SpriteRenderer spriteRenderer = PlayerObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = enabled;
    }

    public static bool Raycast(string layerName, out RaycastHit hit)
    {
        return PlayerMovement.Raycast(layerName, out hit);
    }

    public static bool IsFirstRaycastHit(GameObject obj)
    {
        return PlayerMovement.IsFirstRaycastHit(obj);
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
        if (!DataUtils.SaveFolderExists(PlayerData.FolderName))
        {
            DataUtils.CreateSaveFolder(PlayerData.FolderName);
            return;
        }

        await playerData.LoadDataAsync();
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(PlayerData.FolderName);
    }
    #endregion
}
