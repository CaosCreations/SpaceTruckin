using System;
using UnityEngine;

public enum ShipDamageType
{
    Hull, Engine
}

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]
public class ShipDamageOutcome : MissionOutcome, IOutcomeBreakdown
{
    [SerializeField] private int shipDamage;
    public int BaseDamage => shipDamage;

    [SerializeField] private ShipDamageType damageType;
    public ShipDamageType DamageType => !hasRandomDamageType
        ? damageType
        : ShipUtils.GetRandomDamageType();

    [SerializeField] private bool hasRandomDamageType;

    private int damageTaken;
    private int damageReduced;

    public override void Process(ScheduledMission scheduled)
    {
        damageTaken = (int)(BaseDamage * (1 - LicencesManager.ShipDamageEffect));
        damageReduced = BaseDamage - damageTaken;

        ShipsManager.DamageShip(scheduled.Pilot.Ship, Math.Max(0, damageTaken));

        if (scheduled.MissionToArchive != null)
        {
            // Archive the earnings stats 
            ArchiveOutcomeElements(scheduled);
        }

        LogOutcomeElements();
        ResetOutcomeElements();
    }

    public void ArchiveOutcomeElements(ScheduledMission scheduled)
    {
        scheduled.MissionToArchive.ShipChanges.DamageTaken += damageTaken;
        scheduled.MissionToArchive.ShipChanges.DamageReduced += damageReduced;
        scheduled.MissionToArchive.ShipChanges.DamageType = DamageType;

        // Todo: Replace with nested object
        scheduled.Mission.MissionToArchive.TotalDamageTaken += damageTaken;
        scheduled.Mission.MissionToArchive.TotalDamageReduced += damageReduced;
        scheduled.Mission.MissionToArchive.DamageType = DamageType;
    }

    public void LogOutcomeElements()
    {
        Debug.Log("Base ship damage: " + BaseDamage);
        Debug.Log("Ship damage reduction from licences: " + damageReduced);
        Debug.Log("Total ship damage taken: " + damageTaken);
    }

    public void ResetOutcomeElements()
    {
        damageTaken = damageReduced = default;
    }
}
