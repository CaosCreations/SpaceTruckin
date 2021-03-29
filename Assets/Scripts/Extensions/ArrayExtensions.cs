using System;

public static class ArrayExtensions
{
    public static T[] Shuffle<T>(this T[] self)
    {
        Random random = new Random();
        int n = self.Length;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = self[n];
            self[n] = self[k];
            self[k] = temp;
        }
        return self; 
    }

    public static bool IsNullOrEmpty<T>(this T[] self)
    {
        return self == null || self.Length <= 0;
    }
}
