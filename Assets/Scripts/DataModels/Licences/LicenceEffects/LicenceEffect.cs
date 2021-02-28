using UnityEngine;

public abstract class LicenceEffect : ScriptableObject 
{
    [SerializeField] private string effectName;
    [SerializeField] private double effect;
    public string Name { get => effectName; }
    public double Effect { get => effect; }
    public double Percentage { get => System.Math.Round(Effect * 100, 2); }
}
