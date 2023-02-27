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
        StringBuilder builder = new StringBuilder();
        string missionIdentifierText = $"{mission.Pilot.Name} of the {mission.Pilot.Ship.Name} completed the mission {mission.MissionName}!";
        string moneyText = $"{mission.Pilot.Name} earned ${mission.TotalMoneyEarned} in total.";
        string moneyBonusesText = $"{mission.Pilot.Name} earned ${mission.TotalMoneyIncreaseFromBonuses.RoundTo2()} from bonuses.";
        string moneyLicencesText = $"{mission.Pilot.Name} earned ${mission.TotalMoneyIncreaseFromLicences.RoundTo2()} from licences.";
        string damageText = $"{mission.Pilot.Ship.Name} took {mission.TotalDamageTaken} damage to its {mission.DamageType}.";
        string fuelText = $"{mission.Pilot.Ship.Name} lost {mission.TotalFuelLost} fuel.";
        string xpText = $"{mission.Pilot.Name} gained {mission.TotalPilotXpGained.RoundTo2()} xp in total.";
        string xpBonusesText = $"{mission.Pilot.Name} gained {mission.TotalXpIncreaseFromBonuses.RoundTo2()} xp from bonuses.";
        string missionsCompletedText = $"{mission.Pilot.Name} has now completed {mission.MissionsCompletedByPilotAtTimeOfMission} missions.";

        builder.AppendLineWithBreaks(missionIdentifierText);
        builder.AppendLineWithBreaks(moneyText);

        // Show additional money sources if they exist 
        if (mission.TotalMoneyIncreaseFromBonuses > 0)
            builder.AppendLineWithBreaks(moneyBonusesText);

        if (mission.TotalMoneyIncreaseFromLicences > 0)
            builder.AppendLineWithBreaks(moneyLicencesText);

        builder.AppendLineWithBreaks(damageText);
        builder.AppendLineWithBreaks(fuelText);
        builder.AppendLineWithBreaks(xpText);

        // Show additional XP sources if they exist 
        if (mission.TotalXpIncreaseFromBonuses > 0)
            builder.AppendLineWithBreaks(xpBonusesText);

        builder.AppendLineWithBreaks(missionsCompletedText);

        // Check if the pilot levelled up
        if (mission.PilotLevelAtTimeOfMission < mission.Pilot.Level)
        {
            builder.AppendLineWithBreaks($"{mission.Pilot.Name} has levelled up! (now level {mission.Pilot.Level}).");
        }

        return builder.ToString();
    }
}
