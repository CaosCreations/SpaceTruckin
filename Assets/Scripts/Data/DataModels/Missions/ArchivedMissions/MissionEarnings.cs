using System;

[Serializable]
public class MissionEarnings
{
    public double BaseEarnings { get; set; }
    public double BonusesEarnings { get; set; }
    public double LicencesEarnings { get; set; }

    public double TotalEarnings => BaseEarnings + BonusesEarnings + LicencesEarnings;
}