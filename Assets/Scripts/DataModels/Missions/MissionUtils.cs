using System.Linq;

public static class MissionUtils
{
    public static T GetOutcomeByType<T>(MissionOutcome[] outcomes) where T : class, new()
    {
        return outcomes
            .FirstOrDefault(x => x.GetType() == typeof(T)) as T;
    }
}
