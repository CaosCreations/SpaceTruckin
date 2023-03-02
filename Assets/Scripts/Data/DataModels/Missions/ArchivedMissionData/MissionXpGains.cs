using Newtonsoft.Json;
using System;

[Serializable]
public class MissionXpGains
{
    public double BaseXpGain { get; set; }
    public double LicencesXpGain { get; set; }
    public double BonusesXpGain { get; set; }
    public double TotalXpAfterMission { get; set; }

    [JsonIgnore]
    public double TotalXpGain => BaseXpGain + LicencesXpGain + BonusesXpGain;
}