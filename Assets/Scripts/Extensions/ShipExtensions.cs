public static class ShipExtensions
{
    public static void DeductFuel(this Ship self)
    {
        if (self.CurrentMission != null)
        {
            self.CurrentFuel = System.Math.Max(
                0, self.CurrentFuel - self.CurrentMission.FuelCost);
        }
    }
}
