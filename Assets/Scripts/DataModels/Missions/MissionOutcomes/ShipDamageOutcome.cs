using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]

public class ShipDamageOutcome : MissionOutcome
{   
    [SerializeField] private int shipDamage;

    public int Damage { get => shipDamage; }

    public override void Process(Mission mission)
    {
        int shipDamageTaken = (int)(shipDamage * (1 - LicencesManager.ShipDamageEffect));
        int damageReduced = shipDamage - shipDamageTaken;
        ShipsManager.DamageShip(mission.Ship, Math.Max(0, shipDamageTaken));
        
        if (mission.MissionToArchive != null)
        {
            mission.MissionToArchive.TotalDamageTaken += shipDamageTaken;
            mission.MissionToArchive.TotalDamageReduced += damageReduced;
        }
        Debug.Log("Base ship damage: " + shipDamage);
        Debug.Log("Ship damage reduction from licences: " + damageReduced.ToString());
        Debug.Log("Total ship damage taken: " + shipDamageTaken);
    }
}
