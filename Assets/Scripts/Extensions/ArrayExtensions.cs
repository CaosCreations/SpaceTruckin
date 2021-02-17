using System.Linq;
using System;

public static class ArrayExtensions
{
    public static object[] Shuffle(this object[] self)
    {
        Random random = new Random();
        return self.OrderBy(x => random.Next()).ToArray();
    }
}
