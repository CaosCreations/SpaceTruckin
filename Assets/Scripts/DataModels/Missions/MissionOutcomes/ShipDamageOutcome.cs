 
public class ShipDamageOutcome : MissionOutcome
{   
    public int shipDamage;

    public override void Process(Mission mission)
    {
        ShipsManager.DamageShip(mission.missionSaveData.ship.pilot.id, shipDamage);
    }
}
