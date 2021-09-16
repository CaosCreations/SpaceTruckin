public class ShipUnderRepair
{
    public Ship Ship;
    public ShipDamageType DamageType;

    public Pilot Pilot => Ship.Pilot;
    public bool IsFullyRepaired => Ship.IsFullyRepaired;
    public float HullPercentage => Ship.GetHullPercentage();

    public void Reset()
    {
        Ship = null;
    }
}
