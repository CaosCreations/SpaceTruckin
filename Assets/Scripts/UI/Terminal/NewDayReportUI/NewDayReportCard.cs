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
        shipAvatar.sprite = mission.ShipUsed.Avatar;
        detailsText.text = BuildReportDetails(mission);
    }

    public void SetupNextCardListener(ArchivedMission mission)
    {
        nextCardButton.AddOnClick(() => ShowReport(mission));
    }

    public string BuildReportDetails(ArchivedMission mission)
    {
        StringBuilder builder = new StringBuilder();
        string moneyText = $"{mission.ShipUsed.Name} earned ${mission.TotalMoneyEarned}";
        string damageText = $"{mission.ShipUsed.Name} took {mission.TotalDamageTaken} damage.";
        string fuelText = $"{mission.ShipUsed.Name} lost {mission.TotalFuelLost} fuel";
        string xpText = $"{mission.ShipUsed.Pilot.Name} gained {mission.TotalPilotXpGained}";

        builder.AppendLine(moneyText);
        builder.AppendLine(damageText);
        builder.AppendLine(fuelText);
        builder.AppendLine(xpText);
        return builder.ToString();
    }
}
