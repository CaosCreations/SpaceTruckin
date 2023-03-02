using System;

[Serializable]
public class MissionXpGains/* : IArchivedMissionStats*/
{
    public double BaseXpGain { get; set; }
    public double LicencesXpGain { get; set; }
    public double BonusesXpGain { get; set; }
    public double TotalXpAfterMission { get; set; }

    //public double TotalXpGain { get; set; }

    //public void AddStats(double baseXpGain, double licencesXpGain, double bonusesXpGain)
    //{

    //}
}