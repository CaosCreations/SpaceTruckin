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
        if (mission.Ship.Avatar != null)
        {
            shipAvatar.sprite = mission.Ship.Avatar;
        }
        detailsText.SetText(BuildReportDetails(mission));
    }

    public void SetupNextCardListener(ArchivedMission mission)
    {
        nextCardButton.AddOnClick(() => ShowReport(mission));
    }

    public string BuildReportDetails(ArchivedMission mission)
    {
        StringBuilder builder = new StringBuilder();
        string missionIdentifierText = $@"{mission.Pilot.Name} of the {mission.Ship.Name}
            completed the mission {mission.MissionName}.";

        string moneyText = $"{mission.Ship.Name} earned ${mission.TotalMoneyEarned}.";
        string damageText = $"{mission.Ship.Name} took {mission.TotalDamageTaken} damage.";
        string fuelText = $"{mission.Ship.Name} lost {mission.TotalFuelLost} fuel.";
        string xpText = $"{mission.Pilot.Name} gained {mission.TotalPilotXpGained} xp.";

        builder.AppendLineWithBreaks(missionIdentifierText);
        builder.AppendLineWithBreaks(moneyText);
        builder.AppendLineWithBreaks(damageText);
        builder.AppendLineWithBreaks(fuelText);
        builder.AppendLineWithBreaks(xpText);

        // Check if the pilot levelled up
        if (mission.PilotLevelAtTimeOfMission < mission.Pilot.Level)
        {
            builder.AppendLineWithBreaks($"{mission.Pilot.Name} has levelled up! (now level {mission.Pilot.Level}).");
        }

        return builder.ToString();
    }
}
