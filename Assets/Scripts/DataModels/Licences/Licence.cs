using UnityEngine;

[CreateAssetMenu(fileName = "Licence", menuName = "ScriptableObjects/Licences/Licence", order = 1)]
public class Licence : ScriptableObject
{
    public string licenceName;
    public string description;
    public int tier;
    public int maximumPoints; 
    public int pointsInvested; 
    public int[] costs;
    public LicenceEffect effect;
    public bool HasBeenInvestedInto { get => pointsInvested > 0; }
}
