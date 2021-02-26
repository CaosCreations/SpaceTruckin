public static class LicenceUtils 
{
    public static float CoefficientToPercentage(double coefficient)
    {
        return (float)(1 - coefficient) * 100f;
    }

    public static float CoefficientToPercentage(float coefficient)
    {
        return (1 - coefficient) * 100f;
    }
}
