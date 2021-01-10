using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class Ship : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set In Editor")]
    private ShipData data;

    [Header("Data to update IN GAME")]
    private ShipSaveData saveData;

    public string FOLDER_NAME = "ShipsSaveData";

    public class ShipData
    {
        public string shipName;
        public float maxHullIntegrity;
        public int maxFuel;

        public GameObject shipPrefab;
        public Sprite shipAvatar;
        public Pilot pilot;
    }
    public class ShipSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool isOwned, isLaunched;
        [SerializeField] public int currentFuel;
        [SerializeField] public float currenthullIntegrity;
        [SerializeField] public HangarNode hangarNode;
        [SerializeField] public Mission currentMission;
    }


    public bool IsOwned
    {
        get { return saveData.isOwned; } set { saveData.isOwned = value; }
    }

    public bool IsLaunched
    {
        get { return saveData.isLaunched; } set { saveData.isLaunched = value; }
    }

    public int CurrentFuel
    {
        get { return saveData.currentFuel; } set { saveData.currentFuel = value; }
    }

    public float CurrentHullIntegrity
    {
        get { return saveData.currenthullIntegrity; } set { saveData.currenthullIntegrity = value; }
    }

    public HangarNode HangarNode
    {
        get { return saveData.hangarNode; } set { saveData.hangarNode = value; }
    }

    public Mission CurrentMission
    {
        get { return saveData.currentMission; } set { saveData.currentMission = value; }
    }

    public float GetHullPercent()
    {
        return saveData.currenthullIntegrity / data.maxHullIntegrity;
    }

    public float GetFuelPercent()
    {
        return (float)saveData.currentFuel / data.maxFuel;
    }

    public void SaveData()
    {
        string fileName = $"{data.shipName}_{saveData.guid}";
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = $"{data.shipName}_{saveData.guid}";
        saveData = await DataModelsUtils.LoadFileAsync<ShipSaveData>(fileName, FOLDER_NAME);
    }
}
