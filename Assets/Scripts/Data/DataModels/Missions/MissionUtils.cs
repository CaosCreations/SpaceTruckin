using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MissionUtils
{
    public static T GetOutcomeByType<T>(MissionOutcome[] outcomes) where T : class, new()
    {
        return outcomes
            .FirstOrDefault(x => x.GetType() == typeof(T)) as T;
    }

    public static MissionOutcome[] GetRandomOutcomes(MissionOutcome[] outcomesToChooseFrom)
    {
        var randomOutcomes = new List<MissionOutcome>();

        // Shuffle the Outcomes array so that they will be randomly picked
        outcomesToChooseFrom.Shuffle();

        // Get an Outcome of each type 
        MoneyOutcome moneyOutcome = GetOutcomeByType<MoneyOutcome>(outcomesToChooseFrom);
        PilotXpOutcome pilotXpOutcome = GetOutcomeByType<PilotXpOutcome>(outcomesToChooseFrom);
        OmenOutcome omenOutcome = GetOutcomeByType<OmenOutcome>(outcomesToChooseFrom);
        ShipDamageOutcome shipDamageOutcome = GetOutcomeByType<ShipDamageOutcome>(outcomesToChooseFrom);

        if (moneyOutcome != null) randomOutcomes.Add(moneyOutcome);
        if (pilotXpOutcome != null) randomOutcomes.Add(pilotXpOutcome);
        if (omenOutcome != null) randomOutcomes.Add(omenOutcome);
        if (shipDamageOutcome != null) randomOutcomes.Add(shipDamageOutcome);

        return randomOutcomes.ToArray();
    }

    public static List<Mission> GetMissionsForCustomer(string customerName)
    {
        var missionsForCustomer = new List<Mission>();

        foreach (var mission in MissionsManager.Instance.Missions)
        {
            if (mission != null
                && mission.Customer.Equals(customerName, StringComparison.CurrentCultureIgnoreCase))
            {
                missionsForCustomer.Add(mission);
            }
        }

        return missionsForCustomer;
    }

    public static Mission GetMissionForCustomer(string missionName, string customerName)
    {
        foreach (var mission in MissionsManager.Instance.Missions)
        {
            if (mission != null
                && mission.Name.Equals(missionName, StringComparison.CurrentCultureIgnoreCase)
                && mission.Customer.Equals(customerName, StringComparison.CurrentCultureIgnoreCase))
            {
                return mission;
            }
        }

        Debug.LogWarning($"{customerName}'s mission '{missionName}' cannot be found");
        return null;
    }

    public static void LogMissionOutcomeBreakdown(params object[] outcomeElements)
    {
        foreach (var element in outcomeElements)
        {
            Debug.Log($"{nameof(element)}: {element}");
        }
    }
}
