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
        double baseMoneyEarned = UnityEngine.Random.Range(moneyMin, moneyMax);

        // Apply increases to money from Licences/Bonuses 
        double earningsAfterLicences = baseMoneyEarned * (1 + LicencesManager.MoneyEffect);
        double earningsAfterBonuses = earningsAfterLicences * (1 + scheduled.Bonus.MoneyExponent);

        // Calculate the individual increases 
        double moneyIncreaseFromLicences = earningsAfterLicences - baseMoneyEarned;
        double moneyIncreaseFromBonuses = earningsAfterBonuses - earningsAfterLicences;

        // Convert to the same type as the Player's balance 
        long totalEarnings64 = Convert.ToInt64(earningsAfterBonuses);

        PlayerManager.Instance.ReceiveMoney(totalEarnings64);
 
        // Calculate the total increase 
        long totalMoneyIncrease64 = Convert.ToInt64(totalEarnings64 - baseMoneyEarned);

        if (scheduled.MissionToArchive != null)
        {
            // Archive the earnings stats 
            scheduled.MissionToArchive.TotalMoneyIncreaseFromLicences += moneyIncreaseFromLicences;
            scheduled.MissionToArchive.TotalMoneyIncreaseFromBonuses += moneyIncreaseFromBonuses;
            scheduled.MissionToArchive.TotalAdditionalMoneyEarned += totalMoneyIncrease64;
            scheduled.MissionToArchive.TotalMoneyEarned += totalEarnings64;
        }

        // Log results 
        Debug.Log($"Base money earned: {baseMoneyEarned}");
        Debug.Log($"Money increase due to licences: {moneyIncreaseFromLicences}");
        Debug.Log($"Money increase due to bonuses: {moneyIncreaseFromBonuses}");
        Debug.Log($"Total money earned: {totalEarnings64}");
    }
}
