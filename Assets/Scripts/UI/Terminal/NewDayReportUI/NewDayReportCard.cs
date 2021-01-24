using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NewDayReportCard : MonoBehaviour
{
    private Image shipAvatar;
    private Text detailsText;
    private Button nextCardButton; 

    private void Start()
    {
        shipAvatar = GetComponentInChildren<Image>();
        detailsText = GetComponentInChildren<Text>();
        nextCardButton = GetComponentInChildren<Button>();
    }

    public void ShowReport(Mission mission)
    {
        shipAvatar.sprite = mission.Ship.Avatar;
        detailsText.text = BuildReportDetails(mission);
    }

    public void SetupNextCardListener(Mission nextMission)
    {
        nextCardButton.AddOnClick(() => ShowReport(nextMission));
    }

    public string BuildReportDetails(Mission mission)
    {
        StringBuilder builder = new StringBuilder();
        string moneyText = $"{mission.Ship} earned $";
        string damageText = $"{mission.Ship} took ";
        string xpText = $"{mission.Ship.Pilot} gained ";

        foreach (MissionOutcome outcome in mission.Outcomes)
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
                else
                {
                }
                if (!string.IsNullOrEmpty(moneyText)) builder.AppendLine(moneyText);
                if (!string.IsNullOrEmpty(damageText)) builder.AppendLine(damageText);
                if (!string.IsNullOrEmpty(xpText)) builder.AppendLine(xpText);
            }
        }
        return builder.ToString();
    }
}
