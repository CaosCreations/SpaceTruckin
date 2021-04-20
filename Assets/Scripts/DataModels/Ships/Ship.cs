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

    [Header("Data to update IN GAME")]
    public ShipSaveData saveData;

    public const string FOLDER_NAME = "ShipSaveData";

    [Serializable]
    public class ShipSaveData
    {
        public int currentFuel;
        public float currentHullIntegrity;
        public bool canWarp;
    }
    
    public float GetHullPercent()
    {
        return saveData.currentHullIntegrity / maxHullIntegrity;
    }

    public float GetFuelPercent()
    {
        return (float)saveData.currentFuel / maxFuel;
    }

    public void DeductFuel()
    {
        if (CurrentMission != null)
        {
            CurrentFuel = Math.Max(0, CurrentFuel - CurrentMission.FuelCost);
        }
        else
        {
            Debug.Log($" {CurrentMission.Name} was null when calling Ship.DeductFuel (ln 40) at {DateTime.Now}");
        }
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<ShipSaveData>(name, FOLDER_NAME);
    }
}
