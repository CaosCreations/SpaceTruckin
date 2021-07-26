using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome, IBonusable, IOutcomeBreakdown
{
    [SerializeField] private int xpMin;
    [SerializeField] private int xpMax;

    public int XpMin { get => xpMin; set => xpMin = value; }
    public int XpMax { get => xpMax; set => xpMax = value; }

    // Used for applying various additional XP gains and archiving stats 
    private double baseXpGained, xpAfterOmens, xpAfterLicences, xpAfterBonuses, totalXpGained,
        xpIncreaseFromLicences, xpIncreaseFromBonuses, totalAdditionalXp;

    public override void Process(ScheduledMission scheduled)
    {
        // Store the pilot's level before the xp is awarded.
        // Then we can check if they levelled up as a result of the Mission.
        scheduled.MissionToArchive.PilotLevelAtTimeOfMission = scheduled.Pilot.Level;

        baseXpGained = Random.Range(xpMin, xpMax);

        // Apply xp increases/decreases from Omens/Licences/Bonuses (in that order)
        xpAfterOmens = baseXpGained * ApplyOmens(scheduled);
        xpAfterLicences = xpAfterOmens * (1 + LicencesManager.PilotXpEffect);

        // Calculate the increases
        xpIncreaseFromLicences = xpAfterLicences - xpAfterOmens;
        totalAdditionalXp = xpAfterLicences - baseXpGained;

        totalXpGained = xpAfterLicences;

        // Calculate Bonuses if they exist and the criteria are met 
        if (scheduled.Bonus != null && scheduled.Bonus.AreCriteriaMet(scheduled))
        {
            ApplyBonuses(scheduled);
        }

        if (scheduled.MissionToArchive != null)
        {
            // Archive Pilot XP stats 
            ArchiveOutcomeElements(scheduled);
        }

        LogOutcomeElements();
        ResetOutcomeElements();
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

    public void ApplyBonuses(ScheduledMission scheduled)
    {
        xpAfterBonuses = xpAfterLicences * (1 + scheduled.Bonus.XpExponent);
        xpIncreaseFromBonuses = xpAfterBonuses - xpAfterLicences;
        totalAdditionalXp += xpIncreaseFromBonuses;
        totalXpGained = xpAfterBonuses;
    }

    public void ArchiveOutcomeElements(ScheduledMission scheduled)
    {
        scheduled.MissionToArchive.TotalXpIncreaseFromLicences += xpIncreaseFromLicences;
        scheduled.MissionToArchive.TotalXpIncreaseFromBonuses += xpIncreaseFromBonuses;
        scheduled.MissionToArchive.TotalAdditionalXpGained += totalAdditionalXp;
        scheduled.MissionToArchive.TotalPilotXpGained += totalXpGained;

        scheduled.MissionToArchive.TotalXpAfterMission += PilotsManager.AwardXp(
            scheduled.Pilot, xpAfterBonuses);
    }

    public void LogOutcomeElements()
    {
        Debug.Log($"Base pilot xp gained: {baseXpGained}");
        Debug.Log($"Pilot xp increase from licences: {xpIncreaseFromLicences}");
        Debug.Log($"Pilot xp increase from bonuses: {xpIncreaseFromBonuses}");
        Debug.Log($"Total additional xp gained: {totalAdditionalXp}");
        Debug.Log($"Total pilot xp gained: {totalXpGained}");
    }

    public void ResetOutcomeElements()
    {
        // Default values to 0
        baseXpGained = xpAfterOmens = xpAfterLicences = xpAfterBonuses = totalXpGained =
        xpIncreaseFromLicences = xpIncreaseFromBonuses = totalAdditionalXp = default;
    }
}
