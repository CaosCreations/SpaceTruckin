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
}
