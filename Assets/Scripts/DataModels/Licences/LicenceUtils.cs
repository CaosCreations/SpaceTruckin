using UnityEngine;

public static class LicenceUtils 
{
    public static float CoefficientToPercentage(double coefficient)
    {
        return Mathf.Abs((float)(1 - coefficient) * 100f);
    }

    public static float CoefficientToPercentage(float coefficient)
    {
        return Mathf.Abs((1 - coefficient) * 100f);
    }
}
