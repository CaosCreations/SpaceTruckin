using System;

[Serializable]
public class MissionShipChanges
{
    public int DamageTaken { get; set; }
    public int DamageReduced { get; set; }
    public int FuelLost { get; set; }
    public ShipDamageType DamageType { get; set; }
}