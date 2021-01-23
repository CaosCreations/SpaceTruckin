using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public partial class Ship : ScriptableObject, IDataModel
{
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

    public void CompleteMission()
    {

    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<ShipSaveData>(name, FOLDER_NAME);
    }
}
