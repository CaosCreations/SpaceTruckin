using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the presentation of details for a single recently completed mission.
/// </summary>
public class NewDayReportCard : MonoBehaviour
{
    public Image shipAvatar;
    public Text detailsText;
    public Button nextCardButton;

    public void ShowReport(ArchivedMission mission)
    {
        if (mission != null
            && mission.Pilot != null
            && mission.Pilot.Ship != null
            && mission.Pilot.Avatar != null)
        {
            shipAvatar.sprite = mission.Pilot.Ship.Avatar;

            detailsText.SetText(BuildReportDetails(mission));
        }
    }

    public string BuildReportDetails(ArchivedMission mission)
    {
        StringBuilder builder = new();
        string missionIdentifierText = $"{mission.Pilot.Name} of the {mission.Pilot.Ship.Name} completed the mission \"{mission.Mission.Name}\"!";
        string moneyText = $"{mission.Pilot.Name} earned ${mission.Earnings.TotalEarnings.RoundTo2()} in total.";
        string moneyBonusesText = $"{mission.Pilot.Name} earned ${mission.Earnings.BonusesEarnings.RoundTo2()} from bonuses.";
        string moneyLicencesText = $"{mission.Pilot.Name} earned ${mission.Earnings.LicencesEarnings.RoundTo2()} from licences.";
        string damageText = $"{mission.Pilot.Ship.Name} took {mission.ShipChanges.DamageTaken} damage to its {mission.ShipChanges.DamageType}.";
        string fuelText = $"{mission.Pilot.Ship.Name} lost {mission.ShipChanges.FuelLost} fuel.";
        string xpText = $"{mission.Pilot.Name} gained {mission.XpGains.TotalXpGain.RoundTo2()} xp in total.";
        string xpBonusesText = $"{mission.Pilot.Name} gained {mission.XpGains.BonusesXpGain.RoundTo2()} xp from bonuses.";
        string xpLicencesText = $"{mission.Pilot.Name} gained {mission.XpGains.LicencesXpGain.RoundTo2()} xp from licences.";
        string missionsCompletedText = $"{mission.Pilot.Name} has now completed {mission.MissionsCompletedByPilotAtTimeOfMission} missions.";

        builder.AppendLineWithBreaks(missionIdentifierText);
        builder.AppendLineWithBreaks(moneyText);

        // Show additional money sources if they exist 
        if (mission.Earnings.BonusesEarnings > 0)
            builder.AppendLineWithBreaks(moneyBonusesText);

        if (mission.Earnings.LicencesEarnings > 0)
            builder.AppendLineWithBreaks(moneyLicencesText);

        builder.AppendLineWithBreaks(damageText);
        builder.AppendLineWithBreaks(fuelText);
        builder.AppendLineWithBreaks(xpText);

        // Show additional XP sources if they exist 
        if (mission.XpGains.BonusesXpGain > 0)
            builder.AppendLineWithBreaks(xpBonusesText);

        if (mission.XpGains.LicencesXpGain > 0)
            builder.AppendLineWithBreaks(xpLicencesText);

        builder.AppendLineWithBreaks(missionsCompletedText);

        // Check if the pilot levelled up
        if (mission.PilotLevelAtTimeOfMission < mission.Pilot.Level)
        {
            builder.AppendLineWithBreaks($"{mission.Pilot.Name} has levelled up! (now level {mission.Pilot.Level}).");
        }

        return builder.ToString();
    }
}
