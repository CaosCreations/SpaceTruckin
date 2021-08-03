public interface IBonus
{
    BonusExponents BonusExponents { get; set; }
    bool AreCriteriaMet(ScheduledMission scheduled);
}
