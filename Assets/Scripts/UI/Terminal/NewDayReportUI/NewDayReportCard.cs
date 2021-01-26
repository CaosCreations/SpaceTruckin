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

    private void Start()
    {
        //shipAvatar = GetComponent<Image>();
        //detailsText = GetComponentInChildren<Text>();
        //nextCardButton = GetComponentInChildren<Button>();
    }

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
        string moneyText = $"{mission.ShipUsed} earned ${mission.TotalMoneyEarned}";
        string damageText = $"{mission.ShipUsed} took {mission.TotalDamageTaken}";
        string fuelText = $"{mission.ShipUsed} lost {mission.TotalFuelLost} fuel";
        string xpText = $"{mission.ShipUsed.Pilot} gained {mission.TotalPilotXpGained}";

        if (!string.IsNullOrEmpty(moneyText)) builder.AppendLine(moneyText);
        if (!string.IsNullOrEmpty(damageText)) builder.AppendLine(damageText);
        if (!string.IsNullOrEmpty(fuelText)) builder.AppendLine(fuelText);
        if (!string.IsNullOrEmpty(xpText)) builder.AppendLine(xpText);

        return builder.ToString();
    }
}
