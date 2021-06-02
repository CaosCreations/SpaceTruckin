using System;

public static class NumberExtensions
{
    /// <summary>
    /// Add a number to itself and wrap around if an upper bound is reached.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="addend">The number to add</param>
    /// <param name="numberToWrapAroundTo">The number that will be set if the upper bound is exceeded</param>
    /// <returns></returns>
    public static int AddAndWrapAround(this int self, int addend, int upperBound, int numberToWrapAroundTo = 0)
    {
        return Math.Max((self + addend) % upperBound, numberToWrapAroundTo);
    }
}
