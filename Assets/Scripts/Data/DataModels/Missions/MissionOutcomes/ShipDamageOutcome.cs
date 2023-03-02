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

    public override void Process(ScheduledMission scheduled, bool isMissionModifierOutcome = false)
    {
        damageTaken = (int)(BaseDamage * (1 - LicencesManager.ShipDamageEffect));
        damageReduced = BaseDamage - damageTaken;

        ShipsManager.DamageShip(scheduled.Pilot.Ship, Math.Max(0, damageTaken));

        if (scheduled.MissionToArchive != null)
        {
            // Archive the earnings stats 
            ArchiveOutcome(scheduled, isMissionModifierOutcome);
        }

        LogOutcome();
        ResetOutcome();
    }

    public void ArchiveOutcome(ScheduledMission scheduled, bool isMissionModifierOutcome)
    {
        var archivedOutcome = new ArchivedShipDamageOutcome(this, damageTaken, damageReduced, DamageType);

        if (isMissionModifierOutcome)
        {
            scheduled.MissionToArchive.ArchivedModifierOutcome.ArchivedMissionOutcomeContainer.ArchivedShipDamageOutcomes.Add(archivedOutcome);
        }
        else
        {
            scheduled.Mission.MissionToArchive.ArchivedOutcomeContainer.ArchivedShipDamageOutcomes.Add(archivedOutcome);
        }
    }

    public void LogOutcome()
    {
        Debug.Log("Base ship damage: " + BaseDamage);
        Debug.Log("Ship damage reduction from licences: " + damageReduced);
        Debug.Log("Total ship damage taken: " + damageTaken);
    }

    public void ResetOutcome()
    {
        damageTaken = damageReduced = default;
    }
}
