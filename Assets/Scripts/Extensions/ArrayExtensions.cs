using System;

public static class ArrayExtensions
{
    private static Random random = new Random();
    public static T[] Shuffle<T>(this T[] self)
    {
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

    public static T GetRandomElement<T>(this T[] self)
    {
        return self[random.Next(self.Length - 1)];
    }
}
