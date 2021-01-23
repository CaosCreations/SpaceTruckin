using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    public static UnityAction<Mission, long> OnMoneyOutcome;

    public float moneyMin;
    public float moneyMax;

    public override void Process(Mission mission)
    {
        long moneyReceived = (long)Random.Range(moneyMin, moneyMax);
        PlayerManager.Instance.ReceiveMoney(moneyReceived);

        OnMoneyOutcome?.Invoke(mission, moneyReceived);
    }
}