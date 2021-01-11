using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public partial class Ship : ScriptableObject, IDataModel
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set In Editor")]
    public string shipName;
    public float maxHullIntegrity;
    public int maxFuel;
    public GameObject shipPrefab;
    public Sprite shipAvatar;
    public Pilot pilot;

    [Header("Data to update IN GAME")]
    public ShipSaveData saveData;

    public static string FOLDER_NAME = "ShipSaveData";

    [Serializable]
    public class ShipSaveData
    {
        [SerializeField] public Guid guid = new Guid();
        [SerializeField] public bool isOwned, isLaunched;
        [SerializeField] public int currentFuel;
        [SerializeField] public float currenthullIntegrity;
        [SerializeField] public HangarNode hangarNode;
        [SerializeField] public Mission currentMission;
    }

    public float GetHullPercent()
    {
        return saveData.currenthullIntegrity / maxHullIntegrity;
    }

    public float GetFuelPercent()
    {
        return (float)saveData.currentFuel / maxFuel;
    }

    public void SaveData()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(shipName, saveData.guid);
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = DataModelsUtils.GetUniqueFileName(shipName, saveData.guid);
        saveData = await DataModelsUtils.LoadFileAsync<ShipSaveData>(fileName, FOLDER_NAME);
    }

    public void SetDefaults()
    {
        saveData = new ShipSaveData()
        {
            guid = new Guid(),
            isOwned = false,
            isLaunched = false,
            currentFuel = 0,
            currenthullIntegrity = 0,
            hangarNode = HangarNode.None,
            currentMission = null
        };
    }
}
