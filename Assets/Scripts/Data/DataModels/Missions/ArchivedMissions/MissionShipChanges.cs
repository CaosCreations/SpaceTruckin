using System;

[Serializable]
public class MissionShipChanges
{
    public int DamageTaken { get; set; }
    public int DamageReduced { get; set; }
    public int FuelLost { get; set; }
    public ShipDamageType DamageType { get; set; }

    public MissionShipChanges()
    {
    }

    public MissionShipChanges(int damageTaken, int damageReduced, int fuelLost, ShipDamageType damageType)
    {
        DamageTaken = damageTaken;
        DamageReduced = damageReduced;
        FuelLost = fuelLost;
        DamageType = damageType;
    }
}