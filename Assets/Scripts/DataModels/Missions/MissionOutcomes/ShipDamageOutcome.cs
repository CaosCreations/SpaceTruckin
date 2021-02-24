using UnityEngine;

[CreateAssetMenu(fileName = "ShipDamageOutcome", menuName = "ScriptableObjects/Missions/Outcomes/ShipDamageOutcome", order = 1)]

public class ShipDamageOutcome : MissionOutcome
{   
    [SerializeField] private int shipDamage;

    public int Damage { get; }

    public override void Process(Mission mission)
    {
        int shipDamageTaken = Mathf.RoundToInt(shipDamage * LicencesManager.ShipDamageEffect);
        ShipsManager.DamageShip(mission.Ship, shipDamageTaken);
        mission.MissionToArchive.TotalDamageTaken += shipDamageTaken;
    }
}
