using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    public float moneyMin;
    public float moneyMax;

    public override void Process(Mission mission)
    {
        PlayerManager.Instance.playerData.playerMoney += (long)Random.Range(moneyMin, moneyMax);
    }
}