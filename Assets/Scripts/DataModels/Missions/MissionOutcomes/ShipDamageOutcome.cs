using UnityEngine;

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]

public class ShipDamageOutcome : MissionOutcome
{   
    [SerializeField] private int shipDamage;

    public int Damage { get; }

    public override void Process(Mission mission)
    {
        ShipsManager.DamageShip(mission.Ship, shipDamage);
        mission.MissionToArchive.TotalDamageTaken += shipDamage;
    }
}
