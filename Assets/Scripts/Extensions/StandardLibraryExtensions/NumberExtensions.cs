using System;

public static class NumberExtensions
{
    /// <summary>
    /// Add a number to itself and wrap around if an upper bound is reached.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="addend">The number to add</param>
    /// <param name="numberToWrapAroundTo">The number that will be set if the upper bound is exceeded</param>
    public static int AddAndWrapAround(this int self, int addend, int upperBound, int numberToWrapAroundTo = 0)
    {
        return Math.Max((self + addend) % upperBound, numberToWrapAroundTo);
    }

    public static int ToInGameSeconds(this int self)
    {
        return self * ClockManager.TickSpeedMultiplier; 
    }

    public static int ToRealTimeSeconds(this int self)
    {
        return self / ClockManager.TickSpeedMultiplier;
    }

    public static float ToRealTimeSeconds(this float self)
    {
        return (float)(self / ClockManager.TickSpeedMultiplier);
    }

    // Accept doubles as these are returned by TimeSpan.TotalSeconds 
    public static float ToRealTimeSeconds(this double self)
    {
        return (float)(self / ClockManager.TickSpeedMultiplier);
    }
}
