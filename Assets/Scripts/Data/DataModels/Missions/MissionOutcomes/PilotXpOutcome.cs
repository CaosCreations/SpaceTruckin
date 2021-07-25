using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome
{
    [SerializeField] private int xpMin;
    [SerializeField] private int xpMax;

    public int XpMin { get => xpMin; set => xpMin = value; }
    public int XpMax { get => xpMax; set => xpMax = value; }

    public override void Process(ScheduledMission scheduled)
    {
        // Store the pilot's level before the xp is awarded.
        // Then we can check if they levelled up as a result of the Mission.
        scheduled.MissionToArchive.PilotLevelAtTimeOfMission = scheduled.Pilot.Level;

        double baseXpGained = Random.Range(xpMin, xpMax);

        // Apply xp increases/decreases from Omens/Licences/Bonuses (in that order)
        double xpAfterOmens = baseXpGained * ApplyOmens(scheduled);
        double xpAfterLicences = xpAfterOmens * (1 + LicencesManager.PilotXpEffect);
        double xpAfterBonuses = xpAfterLicences * (1 + scheduled.Bonus.XpExponent);

        // Calculate the increases
        double xpIncreaseFromLicences = xpAfterLicences - xpAfterOmens;
        double xpIncreaseFromBonuses = xpAfterBonuses - xpAfterLicences;
        double totalAdditionalXp = xpIncreaseFromBonuses - baseXpGained;

        if (scheduled.MissionToArchive != null)
        {
            // Archive Pilot XP stats 
            scheduled.MissionToArchive.TotalXpIncreaseFromLicences += xpIncreaseFromLicences;
            scheduled.MissionToArchive.TotalXpIncreaseFromBonuses += xpIncreaseFromBonuses;
            scheduled.MissionToArchive.TotalAdditionalXpGained += totalAdditionalXp;

            scheduled.MissionToArchive.TotalPilotXpGained += PilotsManager.AwardXp(
                scheduled.Pilot, xpAfterBonuses);
        }

        // Log results 
        Debug.Log($"Base pilot xp gained: {baseXpGained}");
        Debug.Log($"Pilot xp increase from licences: {xpIncreaseFromLicences}");
        Debug.Log($"Pilot xp increase from bonuses: {xpIncreaseFromBonuses}");
        Debug.Log($"Total additional xp gained: {totalAdditionalXp}");
        Debug.Log($"Total pilot xp gained: {xpAfterBonuses}");
    }

    /// <summary>
    /// Calculate the xp multiplier based on probability. 
    /// It can be below 1, resulting in an xp debuff. 
    /// </summary>
    /// <param name="scheduled"></param>
    /// <returns>A number by which the xp gained will be multiplied.</returns>
    private float ApplyOmens(ScheduledMission scheduled)
    {
        float coefficient = 1f;

        for (int i = 0; i < scheduled.Mission.Outcomes.Length; i++)
        {
            if (scheduled.Mission.Outcomes[i] is OmenOutcome)
            {
                OmenOutcome omen = scheduled.Mission.Outcomes[i] as OmenOutcome;
                coefficient += Probability >= Random.Range(0f, 1f) ? omen.Coefficient : omen.Coefficient * -1;
            }
        }
        return coefficient;
    }
}
