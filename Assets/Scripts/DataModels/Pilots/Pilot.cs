using System;
using UnityEngine;

public enum Species
{
    Human, Helicid, Myorijiin, Oshunian, HelmetGuy, Vesta
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public partial class Pilot : ScriptableObject
{
    [Header("Set in Editor")]
    public string pilotName, shipName, description;
    public int hireCost;
    public Species species;
    public Sprite avatar;

    [Header("Data to update IN GAME")]
    public PilotSaveData saveData;

    public static string FOLDER_NAME = "PilotSaveData";

    [Serializable]
    public class PilotSaveData
    {
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
}