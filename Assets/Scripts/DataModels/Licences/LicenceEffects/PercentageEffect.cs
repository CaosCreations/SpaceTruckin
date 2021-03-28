using UnityEngine;

public abstract class PercentageEffect : LicenceEffect
{
    [SerializeField] private double coefficient;
    public double Coefficient => coefficient; 
    public double Percentage => System.Math.Round(Coefficient * 100, 2);
}
