using UnityEngine;

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]

public class ShipDamageOutcome : MissionOutcome
{   
    public int shipDamage;

    public override void Process(Mission mission)
    {
        ShipsManager.DamageShip(mission.Ship, shipDamage);
    }
}
