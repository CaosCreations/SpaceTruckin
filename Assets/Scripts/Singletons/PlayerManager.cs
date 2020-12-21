using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Set In Editor")]
    public PlayerData playerData;

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
        if(playerData == null)
        {
            Debug.LogError("No player data found");
        }
    }

    public bool CanSpendMoney(long amount)
    {
        if (amount < Instance.playerData.playerMoney)
        {
            return true;
        }

        return false;
    }

    public void SpendMoney(long amount)
    {
        Instance.playerData.playerMoney -= amount;
        onFinancialTransaction?.Invoke(Instance.playerData.playerMoney);
    }

    public void ReceiveMoney(long amount)
    {
        Instance.playerData.playerMoney += amount;
        Instance.playerData.playerTotalMoneyAcquired += amount;
        onFinancialTransaction?.Invoke(Instance.playerData.playerMoney);
    }
}
