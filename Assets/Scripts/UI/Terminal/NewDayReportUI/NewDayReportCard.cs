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

    public void ShowReport((Mission mission, Ship ship) missionToReport)
    {
        shipAvatar.sprite = missionToReport.ship.Avatar;
        detailsText.text = BuildReportDetails(missionToReport);
    }

    public void SetupNextCardListener((Mission mission, Ship ship) nextMissionToReport)
    {
        nextCardButton.AddOnClick(() => ShowReport(nextMissionToReport));
    }

    public string BuildReportDetails((Mission mission, Ship ship) missionToReport)
    {
        StringBuilder builder = new StringBuilder();
        string moneyText = $"{missionToReport.ship} earned $";
        string damageText = $"{missionToReport.ship} took ";
        string xpText = $"{missionToReport.ship.Pilot} gained ";

        foreach (MissionOutcome outcome in missionToReport.mission.Outcomes)
        {
            // Todo: Flavour text

            if (outcome != null)
            {
                if (outcome is MoneyOutcome)
                {
                    var moneyOutcome = outcome as MoneyOutcome;
                    moneyText += moneyOutcome.MoneyReceived;
                }
                else if (outcome is ShipDamageOutcome)
                {
                    var damageOutcome = outcome as ShipDamageOutcome;
                    damageText += $"{damageOutcome.Damage} damage";
                }
                else if (outcome is PilotXpOutcome)
                {
                    var xpOutcome = outcome as PilotXpOutcome;
                    xpText += $"{xpOutcome.xpMax} experience";
                }
                if (!string.IsNullOrEmpty(moneyText)) builder.AppendLine(moneyText);
                if (!string.IsNullOrEmpty(damageText)) builder.AppendLine(damageText);
                if (!string.IsNullOrEmpty(xpText)) builder.AppendLine(xpText);
            }
        }
        return builder.ToString();
    }
}
