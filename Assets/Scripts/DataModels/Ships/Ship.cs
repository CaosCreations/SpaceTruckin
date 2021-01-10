using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObjects/Ship", order = 1)]
public class Ship : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set In Editor")]
    public ShipData data;

    [Header("Data to update IN GAME")]
    public ShipSaveData saveData;

    public 
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
        public Guid guid = new Guid();
        public bool isOwned, isLaunched;
        public int currentFuel;
        public float currenthullIntegrity;
        public HangarNode hangarNode;
        public Mission currentMission;
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
        Debug.Log($"Saving mission: {data.shipName}_{saveData.guid}");

        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        if (!Directory.Exists(folderPath))
        {
            try
            {
                Directory.CreateDirectory(folderPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        string filePath = Path.Combine(folderPath, $"{data.shipName}_{saveData.guid}.save");
        string fileContents = JsonUtility.ToJson(saveData);
        try
        {
            File.WriteAllText(filePath, fileContents);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
        }
        Debug.Log($"Finished saving mission: {data.shipName}_{saveData.guid}");

    }

    public void LoadData()
    {
        Debug.Log($"loading level: {data.shipName}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"{data.shipName}_{saveData.guid}.save");
        if (File.Exists(filePath))
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<ShipSaveData>(fileContents);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
        Debug.Log($"Finished loading level: {data.shipName}_{saveData.guid}");
    }

    public void DeleteData()
    {
        Debug.Log($"Deleting mission: {data.shipName}_{saveData.guid}");
        string folderPath = Path.Combine(Application.persistentDataPath, this.GetType().Name);
        string filePath = Path.Combine(folderPath, $"/{data.shipName}_{saveData.guid}.save");
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}
