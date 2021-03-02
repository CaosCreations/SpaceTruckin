using UnityEngine;

public abstract class LicenceEffect : ScriptableObject
{
    [SerializeField] private string effectName;
    [SerializeField] private double effect;
    public string Name => effectName;
    public double Effect => effect;
    public double Percentage => System.Math.Round(Effect * 100, 2);
    public bool IsReductive => GetType().IsSubclassOf(typeof(NegativeLicenceEffect));
}