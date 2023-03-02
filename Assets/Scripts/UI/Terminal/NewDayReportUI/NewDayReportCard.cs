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

            ArchivedMissionViewModel viewModel = ArchivedMissionsManager.GetArchivedMissionViewModel(mission);
            detailsText.SetText(BuildReportDetails(viewModel));
        }
    }

    public string BuildReportDetails(ArchivedMissionViewModel viewModel)
    {
        StringBuilder builder = new();
        string missionIdentifierText = $"{viewModel.Pilot.Name} of the {viewModel.Pilot.Ship.Name} completed the mission \"{viewModel.Mission.Name}\"!";
        string moneyText = $"{viewModel.Pilot.Name} earned ${viewModel.Earnings.TotalEarnings.RoundTo2()} in total.";
        string moneyBonusesText = $"{viewModel.Pilot.Name} earned ${viewModel.Earnings.BonusesEarnings.RoundTo2()} from bonuses.";
        string moneyLicencesText = $"{viewModel.Pilot.Name} earned ${viewModel.Earnings.LicencesEarnings.RoundTo2()} from licences.";
        string damageText = $"{viewModel.Pilot.Ship.Name} took {viewModel.ShipChanges.DamageTaken} damage to its {viewModel.ShipChanges.DamageType}.";
        string fuelText = $"{viewModel.Pilot.Ship.Name} lost {viewModel.ShipChanges.FuelLost} fuel.";
        string xpText = $"{viewModel.Pilot.Name} gained {viewModel.XpGains.TotalXpGain.RoundTo2()} xp in total.";
        string xpBonusesText = $"{viewModel.Pilot.Name} gained {viewModel.XpGains.BonusesXpGain.RoundTo2()} xp from bonuses.";
        string xpLicencesText = $"{viewModel.Pilot.Name} gained {viewModel.XpGains.LicencesXpGain.RoundTo2()} xp from licences.";
        string missionsCompletedText = $"{viewModel.Pilot.Name} has now completed {viewModel.ArchivedPilotInfo.MissionsCompletedAtTimeOfMission} missions.";

        builder.AppendLineWithBreaks(missionIdentifierText);
        builder.AppendLineWithBreaks(moneyText);

        // Show additional money sources if they exist 
        if (viewModel.Earnings.BonusesEarnings > 0)
            builder.AppendLineWithBreaks(moneyBonusesText);

        if (viewModel.Earnings.LicencesEarnings > 0)
            builder.AppendLineWithBreaks(moneyLicencesText);

        builder.AppendLineWithBreaks(damageText);
        builder.AppendLineWithBreaks(fuelText);
        builder.AppendLineWithBreaks(xpText);

        // Show additional XP sources if they exist 
        if (viewModel.XpGains.BonusesXpGain > 0)
            builder.AppendLineWithBreaks(xpBonusesText);

        if (viewModel.XpGains.LicencesXpGain > 0)
            builder.AppendLineWithBreaks(xpLicencesText);

        builder.AppendLineWithBreaks(missionsCompletedText);

        // Check if the pilot leveled up
        if (viewModel.ArchivedPilotInfo.LevelAtTimeOfMission < viewModel.Pilot.Level)
        {
            builder.AppendLineWithBreaks($"{viewModel.Pilot.Name} has leveled up! (now level {viewModel.Pilot.Level}).");
        }

        return builder.ToString();
    }
}
