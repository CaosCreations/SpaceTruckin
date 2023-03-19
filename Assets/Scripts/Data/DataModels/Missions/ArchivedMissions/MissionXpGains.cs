using System;

[Serializable]
public class MissionXpGains
{
    public double BaseXpGain { get; set; }
    public double LicencesXpGain { get; set; }
    public double BonusesXpGain { get; set; }
    public double TotalXpAfterMission { get; set; }

    public double TotalXpGain => BaseXpGain + LicencesXpGain + BonusesXpGain;

    public MissionXpGains()
    {
    }

    public MissionXpGains(double baseXpGain, double licencesXpGain, double bonusesXpGain)
    {
        BaseXpGain = baseXpGain;
        LicencesXpGain = licencesXpGain;
        BonusesXpGain = bonusesXpGain;
    }
}