using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ArchivedMissionOutcomeContainer
{
    public List<ArchivedMoneyOutcome> ArchivedMoneyOutcomes = new();
    public List<ArchivedPilotXpOutcome> ArchivedPilotXpOutcomes = new();
    public List<ArchivedShipDamageOutcome> ArchivedShipDamageOutcomes = new();

    public MissionEarnings GetAggregateEarnings()
    {
        long totalBaseMoneyEarned = default;
        long totalEarnedFromLicences = default;
        long totalEarnedFromBonuses = default;

        foreach (var moneyOutcome in ArchivedMoneyOutcomes)
        {
            totalBaseMoneyEarned += (long)moneyOutcome.TotalEarnings;
            totalEarnedFromLicences += (long)moneyOutcome.LicencesEarnings;
            totalEarnedFromBonuses += (long)moneyOutcome.BonusesEarnings;
        }

        var missionEarnings = new MissionEarnings(totalBaseMoneyEarned, totalEarnedFromLicences, totalEarnedFromBonuses);
        return missionEarnings;
    }

    public MissionXpGains GetAggregateXpGains() 
    {
        double totalBaseXpGained = default;
        double totalLicencesXpGained = default;
        double totalBonusesXpGained = default;

        foreach (var xpOutcome in ArchivedPilotXpOutcomes)
        {
            totalBaseXpGained += xpOutcome.BaseXpGain;
            totalLicencesXpGained += xpOutcome.LicencesXpGain;
            totalBonusesXpGained += xpOutcome.BonusesXpGain;
        }

        var missionXpGains = new MissionXpGains(totalBaseXpGained, totalLicencesXpGained, totalBonusesXpGained);
        return missionXpGains;
    }

    // Todo: Make fuel cost into its own Outcome
    public MissionShipChanges GetAggregateShipChanges(int fuelLost)
    {
        int totalDamageTaken = default;
        int totalDamageReduced = default;

        foreach (var damageOutcome in ArchivedShipDamageOutcomes)
        {
            totalDamageTaken += damageOutcome.DamageTaken;
            totalDamageReduced += damageOutcome.DamageReduced;
        }

        var damageType = (ShipDamageType)ArchivedShipDamageOutcomes.FirstOrDefault()?.DamageType;
        var shipChanges = new MissionShipChanges(totalDamageTaken, totalDamageReduced, fuelLost, damageType);
        return shipChanges;
    }
}