using UnityEngine;

public class Licence : ScriptableObject
{
    public string licenceName;
    public int tier;
    public int pointsInvested; 
    public int[] costs;
    public LicenceEffect effect;
}
