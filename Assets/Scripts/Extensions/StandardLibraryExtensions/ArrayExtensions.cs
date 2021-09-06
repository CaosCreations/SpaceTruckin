﻿using System;
using System.Reflection;

public static class ArrayExtensions
{
    private static readonly Random random = new Random();
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

    public static string[] ReplaceTemplates(this string[] self, IDataModel dataModel = null)
    {
        for (int i = 0; i < self.Length; i++)
        {
            self[i] = self[i].ReplaceGameStateTemplates(dataModel);
        }

        return self;
    }

    public static PropertyInfo[] SortByMatchingPropertyNameKeys(this PropertyInfo[] self, string[] keys)
    {
        PropertyInfo[] sortedProperties = new PropertyInfo[self.Length];

        foreach (PropertyInfo property in self)
        {
            int keyIndex = Array.IndexOf(keys, property.Name);

            sortedProperties[keyIndex] = property;
        }

        return sortedProperties;
    }
}