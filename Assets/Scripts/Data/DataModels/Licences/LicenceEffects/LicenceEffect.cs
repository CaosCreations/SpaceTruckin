using UnityEngine;

public abstract class LicenceEffect : ScriptableObject
{
    [SerializeField] private string effectName;
    public string Name => effectName;
}
