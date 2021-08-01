using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    public static T GetRandomItem<T>(this IEnumerable<T> self)
        => self
        .OrderBy(x => Guid.NewGuid())
        .FirstOrDefault();

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        => self == null || self.Count() <= 0;
}
