using System.Text;
using UnityEngine;

public class NewDayReportCard : MonoBehaviour
{
    //public GameObject reportCardPrefab;

    /// <summary>
    /// The mission whose status is currently being reported 
    /// </summary>
    public Mission Mission { get; }

    public string BuildReportDetails()
    {
        string moneyText = $"{Mission.Ship} earned $";
        string damageText = $"{Mission.Ship} took ";
        string xpText = $"{Mission.Ship.Pilot} gained ";

        foreach (MissionOutcome outcome in Mission.Outcomes)
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
                StringBuilder builder = new StringBuilder();
                if (!string.IsNullOrEmpty(moneyText)) builder.AppendLine(moneyText);
                if (!string.IsNullOrEmpty(damageText)) builder.AppendLine(damageText);
                if (!string.IsNullOrEmpty(xpText)) builder.AppendLine(xpText);

                return builder.ToString();
            }
        }
    }
}
