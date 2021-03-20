using System;
using UnityEngine;

public enum Species
{
    HumanMale, HumanFemale, Helicid, Oshunian, Vesta, Robot
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public partial class Pilot : ScriptableObject
{
    [Header("Set in Editor")]
    public bool isRandom;
    [SerializeField] private string pilotName;
    [SerializeField] private string description;
    [SerializeField] private int hireCost;
    [SerializeField] private float xpThresholdExponent;
    [SerializeField] private Species species;
    [SerializeField] private Ship ship;
    [SerializeField] private Sprite avatar;

    [Header("Data to update IN GAME")]
    public PilotSaveData saveData;

    [Serializable]
    public class PilotSaveData
    {
        [Header("Set in Editor")]
        public string randomName;
        public Species randomSpecies;
        public int level;
        public double requiredXp;

        [Header("Data to update IN GAME")]
        public int missionsCompleted;
        public double currentXp;
        public bool isHired/*,isOnMission*/;
        public Mission currentMission;
    }

    public static string FOLDER_NAME = "PilotSaveData";

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PilotSaveData>(name, FOLDER_NAME);
    }

    public void LevelUp()
    {
        Level++;
        RequiredXp = Math.Pow(RequiredXp, XpThresholdExponent);  
    }
}
