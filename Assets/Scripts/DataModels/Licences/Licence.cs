using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Licence", menuName = "ScriptableObjects/Licences/Licence", order = 1)]
public partial class Licence : ScriptableObject
{
    [Header("Set in Editor")]
    [SerializeField] private string licenceName;
    [SerializeField] private string description;
    [SerializeField] private int tier;
    [SerializeField] private int[] costs;
    [SerializeField] private int maximumPoints; 
    [SerializeField] private LicenceEffect effect;

    public LicenceSaveData saveData;

    [Serializable]
    public class LicenceSaveData
    {
        public int pointsInvested;

    }
}
