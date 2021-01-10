using System;
using UnityEngine;
public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public class Pilot : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set in Editor")]
    public PilotData data;

    [Header("Data to update IN GAME")]
    public PilotSaveData saveData;

    public string FOLDER_NAME { get; private set; } = "PilotSaveData";

    public class PilotData
    {
        public string pilotName, shipName, description;
        int hireCost;
        public Species species;
        public Ship ship;
        public Sprite avatar;
    }

    public class PilotSaveData
    {
        public Guid guid = new Guid();
        public int xp, level, missionsCompleted;
        public bool isHired, isOnMission, isAssignedToShip;
    }


    public int Xp { get { return saveData.xp; } }

    public int Level { get { return saveData.level; } }

    public int MissionsCompleted 
    { 
        get { return saveData.missionsCompleted; } set { saveData.missionsCompleted = value; } 
    }

    public bool IsHired 
    { 
        get { return saveData.isHired; } set { saveData.isHired = value; } 
    }

    public bool IsOnMission 
    {
        get { return saveData.isOnMission; } set { saveData.isOnMission = value; } 
    }

    public bool IsAssignedToShip
    {
        get { return saveData.isAssignedToShip; } set { saveData.isAssignedToShip = value; }
    }

    public void SaveData()
    {
        string fileName = $"{data.pilotName}_{saveData.guid}";
        DataModelsUtils.SaveFileAsync(fileName, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        string fileName = $"{data.pilotName}_{saveData.guid}";
        saveData = await DataModelsUtils.LoadFileAsync<PilotSaveData>(fileName, FOLDER_NAME);
    }
}