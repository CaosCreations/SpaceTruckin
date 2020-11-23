
public class ShipDamageOutcome : MissionOutcome
{   
    public int shipDamage;
    public string flavourText;

    public override void Process(Mission mission)
    {
        // ShipDataSingleton.DamageShip(mission.pilot.Id); 
    }
}
