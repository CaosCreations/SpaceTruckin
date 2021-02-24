using UnityEngine;

[CreateAssetMenu(fileName = "Licence", menuName = "ScriptableObjects/Licences/Licence", order = 1)]
public class Licence : ScriptableObject
{
    public string licenceName;
    public int tier;
    public int pointsInvested; 
    public int[] costs;
    public LicenceEffect effect;
}
