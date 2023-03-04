public class ArchivedMissionModifierViewModel
{
	public MissionModifier Modifier { get; set; }
    public MissionModifierOutcome ModifierOutcome { get; set; }
    public MissionEarnings Earnings { get; }
    public MissionXpGains XpGains { get; }
    public MissionShipChanges ShipChanges { get; }

    // Todo: Move logic into service? Or make consistent with the other VM 
    public ArchivedMissionModifierViewModel(ArchivedMissionModifierOutcome archivedModifierOutcome)
	{
        Modifier = archivedModifierOutcome.Modifier;
        ModifierOutcome = archivedModifierOutcome.ModifierOutcome;
        Earnings = archivedModifierOutcome.ArchivedMissionOutcomeContainer.GetAggregateEarnings();
        XpGains = archivedModifierOutcome.ArchivedMissionOutcomeContainer.GetAggregateXpGains();
        ShipChanges = archivedModifierOutcome.ArchivedMissionOutcomeContainer.GetAggregateShipChanges(0); // Todo: Make fuel cost into its own Outcome
	}
}