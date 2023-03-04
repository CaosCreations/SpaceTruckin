public class ArchivedMissionViewModel
{
    public Mission Mission { get; }
    public Pilot Pilot { get; set; }
    public ArchivedMissionPilotInfo ArchivedPilotInfo { get; set; }
    public MissionEarnings Earnings { get; }
    public MissionXpGains XpGains { get; }
    public MissionShipChanges ShipChanges { get; }

    public ArchivedMissionViewModel(
        Mission mission, 
        Pilot pilot, 
        ArchivedMissionPilotInfo archivedPilotInfo, 
        MissionEarnings earnings, 
        MissionXpGains xpGains, 
        MissionShipChanges shipChanges)
    {
        Mission = mission;
        Pilot = pilot;
        ArchivedPilotInfo = archivedPilotInfo;
        Earnings = earnings;
        XpGains = xpGains;
        ShipChanges = shipChanges;
    }
}