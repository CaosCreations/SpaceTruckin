using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int playerStartingMoney; 
    public long playerMoney;
    public long playerTotalMoneyAcquired;

    public void Init()
    {
        playerMoney = playerStartingMoney;
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