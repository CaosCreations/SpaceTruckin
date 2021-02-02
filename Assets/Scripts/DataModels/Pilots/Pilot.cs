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
    [SerializeField] private string pilotName;
    [SerializeField] private string description;
    [SerializeField] private int hireCost; 
    [SerializeField] private float xpThresholdExponent = 1.5f;
    [SerializeField] private Species species;
    [SerializeField] private Sprite avatar;

    [SerializeField] private PilotSaveData saveData;
    public static string FOLDER_NAME = "PilotSaveData";

    [Serializable]
    public class PilotSaveData
    {
        [Header("Set in Editor")]
        public int level;
        public double requiredXp;

        [Header("Data to update IN GAME")]
        public int missionsCompleted;
        public double currentXp;
        public bool isHired, isOnMission, isAssignedToShip;
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<PilotSaveData>(name, FOLDER_NAME);
    }

    public double GainXp(double xpGained) => CurrentXp += xpGained;
    public bool CanLevelUp() => CurrentXp >= RequiredXp;

    //public bool CanLevelUp()
    //{
    //    return Level <= 1 ? CurrentXp >= InitialRequiredXp : CurrentXp >= RequiredXp;
    //}

    public void LevelUp()
    {
        Level++;
        //if (Level <= 1)
        //{
        //    RequiredXp = InitialRequiredXp;
        //}
        RequiredXp = Math.Pow(RequiredXp, XpThresholdExponent);  
    }
}