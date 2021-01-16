using System;
using UnityEngine;

public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public partial class Pilot : ScriptableObject
{
    [Header("Leave this blank. It is set automatically.")]
    public int id;

    [Header("Set in Editor")]
    public string pilotName, shipName, description;
    public int hireCost;
    public Species species;
    public Ship ship;
    public Sprite avatar;

    [Header("Data to update IN GAME")]
    public PilotSaveData saveData;

    public static string FOLDER_NAME = "PilotSaveData";

    [Serializable]
    public class PilotSaveData
    {
        [SerializeField] public int id; 
        [SerializeField] public int xp, level, missionsCompleted;
        [SerializeField] public bool isHired, isOnMission, isAssignedToShip;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<PilotSaveData>(name, FOLDER_NAME);
    }

    public void SetDefaults()
    {
        saveData = new PilotSaveData()
        {
            //guid = new Guid(),
            xp = 0,
            level = 1,
            missionsCompleted = 0,
            isHired = false,
            isOnMission = false,
            isAssignedToShip = false
        };
    }
}