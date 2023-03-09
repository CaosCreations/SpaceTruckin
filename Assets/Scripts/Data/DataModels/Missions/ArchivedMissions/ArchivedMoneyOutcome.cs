public class ArchivedMoneyOutcome : ArchivedMissionOutcome
{
    public double BaseEarnings { get; set; }
    public double BonusesEarnings { get; set; }
    public double LicencesEarnings { get; set; }

    public double TotalEarnings => BaseEarnings + BonusesEarnings + LicencesEarnings;

    public ArchivedMoneyOutcome(
        MoneyOutcome outcome, 
        double baseEarnings, 
        double bonusesEarnings, 
        double licencesEarnings) : base(outcome)
    {
        BaseEarnings = baseEarnings;
        BonusesEarnings = bonusesEarnings;
        LicencesEarnings = licencesEarnings;
    }
}