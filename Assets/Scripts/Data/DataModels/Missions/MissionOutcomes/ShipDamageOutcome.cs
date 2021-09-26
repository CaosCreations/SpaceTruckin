using System;
using UnityEngine;

public enum ShipDamageType
{
    Hull, Engine
}

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]
public class ShipDamageOutcome : MissionOutcome
{
    [SerializeField] private int shipDamage;
    public int BaseDamage => shipDamage;

    [SerializeField] private ShipDamageType damageType;
    public ShipDamageType DamageType => !hasRandomDamageType
        ? damageType
        : ShipUtils.GetRandomDamageType();

    [SerializeField] private bool hasRandomDamageType;

    public override void Process(ScheduledMission scheduled)
    {
        int shipDamageTaken = (int)(BaseDamage * (1 - LicencesManager.ShipDamageEffect));
        int damageReduced = BaseDamage - shipDamageTaken;

        ShipsManager.DamageShip(scheduled.Pilot.Ship, Math.Max(0, shipDamageTaken));

        if (scheduled.Mission.MissionToArchive != null)
        {
            scheduled.Mission.MissionToArchive.TotalDamageTaken += shipDamageTaken;
            scheduled.Mission.MissionToArchive.TotalDamageReduced += damageReduced;
            scheduled.Mission.MissionToArchive.DamageType = DamageType;
        }

        Debug.Log("Base ship damage: " + BaseDamage);
        Debug.Log("Ship damage reduction from licences: " + damageReduced.ToString());
        Debug.Log("Total ship damage taken: " + shipDamageTaken);
    }
}
