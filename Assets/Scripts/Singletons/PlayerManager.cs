﻿using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataModelManager
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    [SerializeField] private PlayerData playerData;
    private long Money
    {
        get => playerData.PlayerMoney;
        set => playerData.PlayerMoney = value;
    }
    public long TotalMoneyAcquired
    {
        get => playerData.PlayerTotalMoneyAcquired; 
        set => playerData.PlayerTotalMoneyAcquired = value;
    }

    [Header("Set at Runtime")]
    public bool isPaused;
    public PlayerMovement playerMovement;

    public static event System.Action<long> onFinancialTransaction;

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

        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
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
        onFinancialTransaction?.Invoke(Instance.Money);
    }

    public void ReceiveMoney(long amount)
    {
        Instance.Money += amount;
        Instance.TotalMoneyAcquired += amount;
        onFinancialTransaction?.Invoke(Instance.Money);
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
        
    }
}
