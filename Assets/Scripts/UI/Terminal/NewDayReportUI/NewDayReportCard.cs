using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the presentation of details for a single recently completed mission.
/// </summary>
public class NewDayReportCard : MonoBehaviour
{
    public Image ShipAvatar;
    public TextFade DetailsText;
    public Button NextCardButton;

    [SerializeField]
    private MissionModifierReportCard modifierReportCard;

    public void Init()
    {
        DetailsText.Clear();
    }

    public virtual void ShowReport(ArchivedMission archivedMission)
    {
        if (archivedMission == null
            || archivedMission.Pilot == null
            || archivedMission.Pilot.Ship == null
            || archivedMission.Pilot.Avatar == null)
        {
            Debug.LogError("Invalid arguments passed to ShowReport method");
            return;
        }

        ShipAvatar.sprite = archivedMission.Pilot.Ship.Avatar;

        ArchivedMissionViewModel viewModel = new(archivedMission);
        DetailsText.SetTextWithFade(BuildReportDetails(viewModel));

        if (archivedMission.Mission.HasModifier)
        {
            Debug.Log($"{archivedMission.Mission} has modifier. Will show modifier report details next..");
            NextCardButton.AddOnClick(() => ShowMissionModifierReport(archivedMission));
        }
    }

    private string BuildReportDetails(ArchivedMissionViewModel viewModel)
    {
        StringBuilder builder = new();
        builder.AppendLineWithBreaks($"{viewModel.Pilot.Name} of the {viewModel.Pilot.Ship.Name} completed the mission \"{viewModel.Mission.Name}\"!");

        string outcomeDetails = BuildOutcomeDetails(viewModel.Pilot, viewModel.Earnings, viewModel.XpGains, viewModel.ShipChanges);
        builder.AppendLineWithBreaks(outcomeDetails);

        builder.AppendLineWithBreaks($"{viewModel.Pilot.Name} has now completed {viewModel.ArchivedPilotInfo.MissionsCompletedAtTimeOfMission} missions.");

        // Check if the pilot leveled up
        if (viewModel.ArchivedPilotInfo.LevelAtTimeOfMission < viewModel.Pilot.Level)
        {
            builder.AppendLineWithBreaks($"{viewModel.Pilot.Name} has leveled up! (now level {viewModel.Pilot.Level}).");
        }

        return builder.ToString();
    }

    protected string BuildOutcomeDetails(
        Pilot pilot,
        MissionEarnings earnings,
        MissionXpGains xpGains,
        MissionShipChanges shipChanges)
    {
        StringBuilder builder = new();
        string moneyText = $"{pilot.Name} earned ${earnings.TotalEarnings.RoundTo2()} in total.";
        string moneyBonusesText = $"{pilot.Name} earned ${earnings.BonusesEarnings.RoundTo2()} from bonuses.";
        string moneyLicencesText = $"{pilot.Name} earned ${earnings.LicencesEarnings.RoundTo2()} from licences.";
        string damageText = $"{pilot.Ship.Name} took {shipChanges.DamageTaken} damage to its {shipChanges.DamageType}.";
        string fuelText = $"{pilot.Ship.Name} lost {shipChanges.FuelLost} fuel.";
        string xpText = $"{pilot.Name} gained {xpGains.TotalXpGain.RoundTo2()} xp in total.";
        string xpBonusesText = $"{pilot.Name} gained {xpGains.BonusesXpGain.RoundTo2()} xp from bonuses.";
        string xpLicencesText = $"{pilot.Name} gained {xpGains.LicencesXpGain.RoundTo2()} xp from licences.";

        builder.AppendLineWithBreaks(moneyText);

        // Show additional money sources if they exist 
        if (earnings.BonusesEarnings > 0)
            builder.AppendLineWithBreaks(moneyBonusesText);

        if (earnings.LicencesEarnings > 0)
            builder.AppendLineWithBreaks(moneyLicencesText);

        builder.AppendLineWithBreaks(damageText);
        builder.AppendLineWithBreaks(fuelText);
        builder.AppendLineWithBreaks(xpText);

        // Show additional XP sources if they exist 
        if (xpGains.BonusesXpGain > 0)
            builder.AppendLineWithBreaks(xpBonusesText);

        if (xpGains.LicencesXpGain > 0)
            builder.AppendLineWithBreaks(xpLicencesText);

        return builder.ToString();
    }

    private void ShowMissionModifierReport(ArchivedMission archivedMission)
    {
        modifierReportCard.gameObject.SetActive(true);
        modifierReportCard.ShowReport(archivedMission);
        gameObject.SetActive(false);
    }
}
