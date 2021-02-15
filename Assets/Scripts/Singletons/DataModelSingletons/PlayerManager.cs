using System.Linq;
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
    public Color SpriteColour { get => playerData.SpriteColour; }

    [Header("Set at Runtime")]
    public bool isPaused;
    public PlayerMovement playerMovement;
    public PlayerCustomisation playerCustomisation;

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
        if (DataModelsUtils.SaveFolderExists(PlayerData.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(PlayerData.FOLDER_NAME);
        }

        GetPlayerReferences();
        if (playerCustomisation != null)
        {
            playerCustomisation.SetSpriteRendererColour(playerData.SpriteColour);
        }

        if (playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    private static void GetPlayerReferences()
    {
        GameObject playerObject = GameObject.FindGameObjectsWithTag(PlayerConstants.objectTag)
            .FirstOrDefault();

        if (playerObject != null)
        {
            Instance.playerMovement = playerObject.GetComponent<PlayerMovement>();
            Instance.playerCustomisation = playerObject.GetComponent<PlayerCustomisation>();
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
        DataModelsUtils.RecursivelyDeleteSaveData(PlayerData.FOLDER_NAME);
    }

    public void SetSpriteColour(Color newColour)
    {
        playerData.SpriteColour = newColour;
        playerCustomisation.SetSpriteRendererColour(newColour);
    }
}
