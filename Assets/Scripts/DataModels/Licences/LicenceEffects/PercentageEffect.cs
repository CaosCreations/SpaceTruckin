using UnityEngine;

public abstract class PercentageEffect : LicenceEffect
{
    [SerializeField] private double coefficient;
    public double Coefficient => coefficient; 
    public double Percentage => System.Math.Round(coefficient * 100, 2);
}
