using Events;
using System.Text;
using UnityEngine;

public class MissionModifierReportCard : NewDayReportCard
{
    private ArchivedMission archivedMission;

    private void Start()
    {
        NextCardButton.AddOnClick(CloseReport);
    }

    public override void ShowReport(ArchivedMission archivedMission)
    {
        if (archivedMission == null
            || archivedMission.Pilot == null
            || archivedMission.Pilot.Ship == null)
        {
            Debug.LogError("Invalid arguments passed to ShowReport method");
            return;
        }

        this.archivedMission = archivedMission;

        // Get modifier outcome data 
        var viewModel = new ArchivedMissionModifierViewModel(this.archivedMission.ArchivedModifierOutcome);

        // Set text based on the mission modifier outcome that occurred and its sub-outcomes 
        DetailsText.SetText(BuildModifierDetails(viewModel, this.archivedMission.Pilot));

        SingletonManager.EventService.Dispatch<OnModifierReportCardOpenedEvent>();
    }

    private string BuildModifierDetails(ArchivedMissionModifierViewModel viewModel, Pilot pilot)
    {
        StringBuilder builder = new();
        builder.AppendLineWithBreaks(viewModel.Modifier.FlavourText);
        builder.AppendLineWithBreaks(viewModel.ModifierOutcome.FlavourText, 1);
        builder.AppendLineWithBreaks($"{pilot.Name}'s {viewModel.Modifier.DependentAttribute} attribute is {pilot.GetAttributeLevelByType(viewModel.Modifier.DependentAttribute)}, which caused this outcome.");

        var outcomeDetails = BuildOutcomeDetails(pilot, viewModel.Earnings, viewModel.XpGains, viewModel.ShipChanges);
        builder.AppendLineWithBreaks(outcomeDetails);
        return builder.ToString();
    }

    private void CloseReport()
    {
        archivedMission.ArchivedModifierOutcome.HasBeenViewedInReport = true;
        SingletonManager.EventService.Dispatch<OnModifierReportCardClosedEvent>();
        gameObject.SetActive(false);
    }
}