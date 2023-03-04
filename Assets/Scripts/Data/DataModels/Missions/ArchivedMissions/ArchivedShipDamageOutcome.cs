public class ArchivedShipDamageOutcome : ArchivedMissionOutcome
{
    public int DamageTaken { get; set; }
    public int DamageReduced { get; set; }
    public ShipDamageType DamageType { get; set; }

    public int BaseDamageTaken => DamageTaken + DamageReduced;

    public ArchivedShipDamageOutcome(MissionOutcome outcome, int damageTaken, int damageReduced, ShipDamageType damageType) 
        : base(outcome)
    {
        DamageTaken = damageTaken;
        DamageReduced = damageReduced;
        DamageType = damageType;
    }
}