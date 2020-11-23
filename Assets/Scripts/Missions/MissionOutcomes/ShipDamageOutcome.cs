
public class ShipDamageOutcome : MissionOutcome
{   
    public int shipDamage;

    public override void Process(Mission mission)
    {
        // TODO: Ship model and Ship singleton 

        //ShipsManager.ships[mission.pilot.ship.id].hullIntegrity -= shipDamage; 
    }
}
