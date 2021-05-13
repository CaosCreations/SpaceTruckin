using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T> self)
    {
        return self == null || self.Count <= 0;
    }

    public static HashSet<T> ToHashSet<T>(this List<T> self) 
    {
        var hashSet = new HashSet<T>();
        
        self = self.Distinct().ToList();
        self.ForEach(x => hashSet.Add(x));

        return hashSet;
    }
}
