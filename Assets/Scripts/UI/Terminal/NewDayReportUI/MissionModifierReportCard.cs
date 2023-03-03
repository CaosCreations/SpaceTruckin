using Events;
using System.Text;
using UnityEngine;

public class MissionModifierReportCard : NewDayReportCard
{
    public override void ShowReport(ArchivedMission archivedMission)
    {
        if (archivedMission == null
            || archivedMission.Pilot == null
            || archivedMission.Pilot.Ship == null)
        {
            Debug.LogError("Invalid arguments passed to ShowReport method");
            return;
        }

        // Get modifier outcome data 
        var viewModel = new ArchivedMissionModifierViewModel(archivedMission.ArchivedModifierOutcome);

        // Set text based on the mission modifier outcome that occurred and its sub-outcomes 
        detailsText.SetText(BuildModifierDetails(viewModel, archivedMission.Pilot));

        SingletonManager.EventService.Dispatch<OnModifierReportCardOpenedEvent>();
    }

    private string BuildModifierDetails(ArchivedMissionModifierViewModel viewModel, Pilot pilot)
    {
        StringBuilder builder = new();
        builder.AppendLineWithBreaks(viewModel.Modifier.FlavourText);
        builder.AppendLineWithBreaks(viewModel.ModifierOutcome.FlavourText, 1);

        var outcomeDetails = BuildOutcomeDetails(pilot, viewModel.Earnings, viewModel.XpGains, viewModel.ShipChanges);
        builder.AppendLineWithBreaks(outcomeDetails);
        return builder.ToString();
    }
}