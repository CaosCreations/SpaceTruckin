public class ArchivedMissionViewModel
{
    public Mission Mission { get; }
    public Pilot Pilot { get; set; }
    public ArchivedMissionPilotInfo ArchivedPilotInfo { get; set; }
    public MissionEarnings Earnings { get; }
    public MissionXpGains XpGains { get; }
    public MissionShipChanges ShipChanges { get; }
    public bool LevelledUp => ArchivedPilotInfo.LevelAtTimeOfMission < Pilot.Level;

    public ArchivedMissionViewModel(ArchivedMission archivedMission)
    {
        Mission = archivedMission.Mission;
        Pilot = archivedMission.Pilot;
        ArchivedPilotInfo = archivedMission.ArchivedPilotInfo;
        Earnings = archivedMission.ArchivedOutcomeContainer.GetAggregateEarnings();
        XpGains = archivedMission.ArchivedOutcomeContainer.GetAggregateXpGains();
        ShipChanges = archivedMission.ArchivedOutcomeContainer.GetAggregateShipChanges(archivedMission.Mission.FuelCost);
    }
}