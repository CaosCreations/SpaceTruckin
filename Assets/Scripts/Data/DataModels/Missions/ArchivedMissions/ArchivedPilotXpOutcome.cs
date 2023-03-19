public class ArchivedPilotXpOutcome : ArchivedMissionOutcome
{
    public double BaseXpGain { get; set; }
    public double LicencesXpGain { get; set; }
    public double BonusesXpGain { get; set; }
    public float OmenCoefficient { get; set; }

    public double TotalXpGain => BaseXpGain + LicencesXpGain + BonusesXpGain + OmenCoefficient;

    public ArchivedPilotXpOutcome(MissionOutcome outcome, double baseXpGain, double licencesXpGain, double bonusesXpGain, float omenCoefficient) 
        : base(outcome)
    {
        BaseXpGain = baseXpGain;
        LicencesXpGain = licencesXpGain;
        BonusesXpGain = bonusesXpGain;
        OmenCoefficient = omenCoefficient;
    }
}