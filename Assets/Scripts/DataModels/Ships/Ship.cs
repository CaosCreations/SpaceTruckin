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

    private const string FOLDER_NAME = "ShipsSaveData";

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
        get => saveData.isOwned; set => saveData.isOwned = value;
    }

    public bool IsLaunched
    {
        get => saveData.isLaunched; set => saveData.isLaunched = value;
    }

    public int CurrentFuel
    {
        get => saveData.currentFuel; set => saveData.currentFuel = value;
    }

    public int MaxFuel { get => data.maxFuel; set => data.maxFuel = value; }

    public float CurrentHullIntegrity
    {
        get => saveData.currenthullIntegrity; set => saveData.currenthullIntegrity = value;
    }

    public HangarNode HangarNode
    {
        get => saveData.hangarNode; set => saveData.hangarNode = value;
    }

    public Mission CurrentMission
    {
        get => saveData.currentMission; set => saveData.currentMission = value;
    }

    public GameObject ShipPrefab
    {
        get => data.shipPrefab; set => data.shipPrefab = value;
    }

    public Pilot Pilot { get => data.pilot; set => data.pilot = value; }

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
        string fileName = DataModelsUtils.GetUniqueFileName(data.shipName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(data.shipName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<ShipSaveData>(fileName, FOLDER_NAME);
    }
}
