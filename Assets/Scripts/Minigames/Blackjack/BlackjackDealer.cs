public class BlackjackDealer : BlackjackPlayer
{
    private readonly int forcedToStandThreshold = 17;
    public override bool IsStanding { get => handTotal >= forcedToStandThreshold && !IsBust; }
}
