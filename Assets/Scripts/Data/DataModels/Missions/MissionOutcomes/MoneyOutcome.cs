using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome
{
    [SerializeField] private long moneyMin;
    [SerializeField] private long moneyMax;
    public long MoneyMin { get => moneyMin; set => moneyMin = value; }
    public long MoneyMax { get => moneyMax; set => moneyMax = value; }

    public override void Process(ScheduledMission scheduled)
    {
        double moneyEarned = UnityEngine.Random.Range(moneyMin, moneyMax);
        double earningsAfterLicences = moneyEarned * (1 + LicencesManager.MoneyEffect);
        long earnings64 = Convert.ToInt64(earningsAfterLicences);

        PlayerManager.Instance.ReceiveMoney(earnings64);

        long moneyIncrease64 = Convert.ToInt64(earnings64 - moneyEarned);

        if (scheduled.Mission.MissionToArchive != null)
        {
            scheduled.Mission.MissionToArchive.TotalMoneyIncrease += moneyIncrease64;
            scheduled.Mission.MissionToArchive.TotalMoneyEarned += earnings64;
        }
        Debug.Log("Base money earned: " + moneyEarned);
        Debug.Log("Money increase due to licences: " + (earnings64 - moneyEarned).ToString());
        Debug.Log("Total money earned: " + earnings64);
    }
}