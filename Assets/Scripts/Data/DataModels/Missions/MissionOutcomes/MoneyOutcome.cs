using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyOutcome", menuName = "ScriptableObjects/Missions/Outcomes/MoneyOutcome", order = 1)]
public class MoneyOutcome : MissionOutcome, IBonusable, IOutcomeBreakdown
{
    [SerializeField] private long moneyMin;
    [SerializeField] private long moneyMax;
    public long MoneyMin { get => moneyMin; set => moneyMin = value; }
    public long MoneyMax { get => moneyMax; set => moneyMax = value; }

    // Breakdown of the profits for this Mission 
    private double baseMoneyEarned, earningsAfterLicences, moneyIncreaseFromLicences,
        earningsAfterBonuses, moneyIncreaseFromBonuses, totalEarnings;

    // The same type as the Player's balance 
    private long totalEarnings64, totalAdditionalMoneyEarned;

    public override void Process(ScheduledMission scheduled)
    {
        baseMoneyEarned = UnityEngine.Random.Range(moneyMin, moneyMax);

        // Apply earnings from Licences 
        earningsAfterLicences = baseMoneyEarned * (1 + LicencesManager.MoneyEffect);
        moneyIncreaseFromLicences = earningsAfterLicences - baseMoneyEarned;

        totalEarnings = earningsAfterLicences;

        // Apply earnings from Bonuses if they exist and the criteria are met 
        if (scheduled.Bonus != null && scheduled.Bonus.AreCriteriaMet(scheduled))
        {
            ApplyBonuses(scheduled);
        }

        totalEarnings64 = Convert.ToInt64(totalEarnings);

        // Pay the player
        PlayerManager.Instance.ReceiveMoney(totalEarnings64);

        // Calculate the total of additional earnings  
        totalAdditionalMoneyEarned = Convert.ToInt64(totalEarnings64 - baseMoneyEarned);

        if (scheduled.MissionToArchive != null)
        {
            // Archive the earnings stats 
            ArchiveOutcomeElements(scheduled);
        }

        LogOutcomeElements();
        ResetOutcomeElements();
    }

    public void ApplyBonuses(ScheduledMission scheduled)
    {
        earningsAfterBonuses = earningsAfterLicences * (1 + scheduled.Bonus.MoneyExponent);
        moneyIncreaseFromBonuses = earningsAfterBonuses - earningsAfterLicences;

        // Update the total with the added bonuses
        totalEarnings = earningsAfterBonuses;
    }

    public void ArchiveOutcomeElements(ScheduledMission scheduled)
    {
        scheduled.MissionToArchive.TotalMoneyIncreaseFromLicences += moneyIncreaseFromLicences;
        scheduled.MissionToArchive.TotalMoneyIncreaseFromBonuses += moneyIncreaseFromBonuses;
        scheduled.MissionToArchive.TotalAdditionalMoneyEarned += totalAdditionalMoneyEarned;
        scheduled.MissionToArchive.TotalMoneyEarned += totalEarnings64;
    }

    public void LogOutcomeElements()
    {
        Debug.Log($"Base money earned: {baseMoneyEarned}");
        Debug.Log($"Money increase due to licences: {moneyIncreaseFromLicences}");
        Debug.Log($"Money increase due to bonuses: {moneyIncreaseFromBonuses}");
        Debug.Log($"Total additional money earned: {totalAdditionalMoneyEarned}");
        Debug.Log($"Total money earned: {totalEarnings64}");
    }

    public void ResetOutcomeElements()
    {
        // Default all to 0 
        baseMoneyEarned = earningsAfterLicences = moneyIncreaseFromLicences = earningsAfterBonuses
            = moneyIncreaseFromBonuses = totalEarnings = totalEarnings64 = totalAdditionalMoneyEarned = default;
    }
}
