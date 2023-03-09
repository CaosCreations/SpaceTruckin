using UnityEngine;

[CreateAssetMenu(fileName = "PilotXpOutcome", menuName = "ScriptableObjects/Missions/Outcomes/PilotXpOutcome", order = 1)]
public class PilotXpOutcome : MissionOutcome, IBonusable, IOutcomeBreakdown
{
    [SerializeField] private int xpMin;
    [SerializeField] private int xpMax;

    public int XpMax { get => xpMax; set => xpMax = value; }
    public int XpMin { get => xpMin; set => xpMin = value; }

    // Used for applying various additional XP gains and archiving stats 
    private double baseXpGained, xpAfterOmens, xpAfterLicences, xpAfterBonuses, totalXpGained,
        xpIncreaseFromLicences, xpIncreaseFromBonuses, totalAdditionalXp;

    private float omenCoefficient;

    public override void Process(ScheduledMission scheduled, bool isMissionModifierOutcome = false)
    {
        // Store the pilot's level before the xp is awarded.
        // Then we can check if they leveled up as a result of the Mission.
        scheduled.MissionToArchive.ArchivedPilotInfo.LevelAtTimeOfMission = scheduled.Pilot.Level;
        scheduled.MissionToArchive.ArchivedPilotInfo.AttributesAtTimeOfMission = scheduled.Pilot.Attributes;

        baseXpGained = Random.Range(xpMin, xpMax);

        // Apply xp increases/decreases from Omens/Licences/Bonuses (in that order)
        omenCoefficient = ApplyOmens(scheduled);
        xpAfterOmens = baseXpGained * omenCoefficient;
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
            ArchiveOutcome(scheduled, isMissionModifierOutcome);
        }

        LogOutcome();
        ResetOutcome();
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

    public void ArchiveOutcome(ScheduledMission scheduled, bool isMissionModifierOutcome)
    {
        var archivedOutcome = new ArchivedPilotXpOutcome(this, baseXpGained, xpIncreaseFromLicences, xpIncreaseFromBonuses, omenCoefficient);

        if (isMissionModifierOutcome)
        {
            scheduled.MissionToArchive.ArchivedModifierOutcome.ArchivedMissionOutcomeContainer.ArchivedPilotXpOutcomes.Add(archivedOutcome);
        }
        else
        {
            scheduled.Mission.MissionToArchive.ArchivedOutcomeContainer.ArchivedPilotXpOutcomes.Add(archivedOutcome);
        }

        scheduled.MissionToArchive.ArchivedPilotInfo.TotalXpAfterMission += PilotsManager.AwardXp(scheduled.Pilot, totalXpGained);
    }

    public void LogOutcome()
    {
        Debug.Log($"Base pilot xp gained: {baseXpGained}");
        Debug.Log($"Pilot xp increase from licences: {xpIncreaseFromLicences}");
        Debug.Log($"Pilot xp increase from bonuses: {xpIncreaseFromBonuses}");
        Debug.Log($"Total additional xp gained: {totalAdditionalXp}");
        Debug.Log($"Total pilot xp gained: {totalXpGained}");
    }

    public void ResetOutcome()
    {
        // Default values to 0
        baseXpGained = xpAfterOmens = xpAfterLicences = xpAfterBonuses = totalXpGained =
        xpIncreaseFromLicences = xpIncreaseFromBonuses = totalAdditionalXp = default;
    }
}
