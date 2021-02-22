using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    [SerializeField] private long moneyMin;
    [SerializeField] private long moneyMax;
    public long MoneyMin { get => moneyMin; set => moneyMin = value; }
    public long MoneyMax { get => moneyMax; set => moneyMax = value; }

    public override void Process(Mission mission)
    {
        long moneyEarned = (long)Random.Range(moneyMin, moneyMax);
        PlayerManager.Instance.ReceiveMoney(moneyEarned);

        if (mission.MissionToArchive != null)
        {
            mission.MissionToArchive.TotalMoneyEarned += moneyEarned;
        }
    }
}