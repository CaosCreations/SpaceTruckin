using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]

public class ShipDamageOutcome : MissionOutcome
{   
    [SerializeField] private int shipDamage;
    public int Damage => shipDamage;

    public override void Process(ScheduledMission scheduled)
    {
        int shipDamageTaken = (int)(shipDamage * (1 - LicencesManager.ShipDamageEffect));
        int damageReduced = shipDamage - shipDamageTaken;
        ShipsManager.DamageShip(scheduled.Pilot.Ship, Math.Max(0, shipDamageTaken));
        
        if (scheduled.Mission.MissionToArchive != null)
        {
            scheduled.Mission.MissionToArchive.TotalDamageTaken += shipDamageTaken;
            scheduled.Mission.MissionToArchive.TotalDamageReduced += damageReduced;
        }
        Debug.Log("Base ship damage: " + shipDamage);
        Debug.Log("Ship damage reduction from licences: " + damageReduced.ToString());
        Debug.Log("Total ship damage taken: " + shipDamageTaken);
    }
}
