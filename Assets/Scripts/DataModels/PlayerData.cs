using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public long playerMoney;
    public long playerTotalMoneyAcquired; //Used to unlock missions

    public bool CanSpendMoney(long amount)
    {
        if (amount < playerMoney)
        {
            return true;
        }

        return false;
    }

    public void SpendMoney(long amount)
    {
        playerMoney -= amount;
    }

    public void ReceiveMoney(long amount)
    {
        playerMoney += amount;
        playerTotalMoneyAcquired += amount;
    }
}