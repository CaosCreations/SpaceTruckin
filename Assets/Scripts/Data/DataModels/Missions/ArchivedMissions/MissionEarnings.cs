using System;

[Serializable]
public class MissionEarnings
{
    public double BaseEarnings { get; set; }
    public double LicencesEarnings { get; set; }
    public double BonusesEarnings { get; set; }

    public double TotalEarnings => BaseEarnings + LicencesEarnings + BonusesEarnings;

    public MissionEarnings()
    {
    }

    public MissionEarnings(double baseEarnings, double licencesEarnings, double bonusesEarnings)
    {
        BaseEarnings = baseEarnings;
        LicencesEarnings = licencesEarnings;
        BonusesEarnings = bonusesEarnings;
    }
}