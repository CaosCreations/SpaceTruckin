using System.Collections.Generic;
using System.Linq;

public static class ConditionExtensions
{
    public static bool AreAllMet(this IEnumerable<Condition> self)
    {
        return !self.Any() || self.All(c => c.IsMet);
    }
}