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
    [SerializeField] private string pilotName, description;
    [SerializeField] private int hireCost;
    [SerializeField] private double xpThresholdExponent = 1.5d;
    [SerializeField] private Species species;
    [SerializeField] private Sprite avatar;

    [Header("Data to update IN GAME")]
    [SerializeField] private PilotSaveData saveData;

    public static string FOLDER_NAME = "PilotSaveData";

    [Serializable]
    public class PilotSaveData
    {
        [SerializeField] public int level, missionsCompleted;
        [SerializeField] public double currentXp, requiredXp;
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

    public bool CanLevelUp() => CurrentXp >= RequiredXp;

    public void LevelUp()
    {
        Level++;
        RequiredXp = Math.Pow(RequiredXp, XpThresholdExponent);  
        
    }
    
}