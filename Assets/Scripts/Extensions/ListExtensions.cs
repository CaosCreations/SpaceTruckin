using System.Collections.Generic;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T> self)
    {
        return self == null || self.Count <= 0;
    }
}
