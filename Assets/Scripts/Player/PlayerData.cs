using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public static int playerStartingMoney; 
    public static long playerMoney;

    public static void Init()
    {
        playerMoney = playerStartingMoney;
    }
}