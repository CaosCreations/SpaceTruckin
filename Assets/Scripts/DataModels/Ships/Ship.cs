using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class Ship : ScriptableObject
{
    public class ShipSaveData
    {
        public bool isOwned, isLaunched;
        public int currentFuel;
        public float currenthullIntegrity;
        public HangarNode hangarNode;
        public Mission currentMission;
    }

    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set In Editor")]
    public string shipName;
    public float maxHullIntegrity;
    public int maxFuel;

    public GameObject shipPrefab;
    public Sprite shipAvatar; 
    public Pilot pilot;

    [Header("Set at Runtime")]
    public ShipSaveData shipSaveData;

    public float GetHullPercent()
    {
        return shipSaveData.currenthullIntegrity / maxHullIntegrity;
    }

    public float GetFuelPercent()
    {
        return (float)shipSaveData.currentFuel / maxFuel;
    }
}
