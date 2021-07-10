using System;
using System.Threading.Tasks;
using UnityEngine;

public enum Species
{
    HumanMale, HumanFemale, Helicid, Oshunian, Vesta, Robot
}

public enum PilotAttributeType
{
    Navigation, Savviness
}

[Serializable]
public struct PilotAttributes
{
    public int Navigation, Savviness;
}

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilot", order = 1)]
public partial class Pilot : ScriptableObject
{
    [Header("Set in Editor")]
    [SerializeField] private bool isRandom;
    [SerializeField] private string pilotName;
    [SerializeField] private string like;
    [SerializeField] private string dislike;
    [SerializeField] private int hireCost;
    [SerializeField] private int levelsNeededForAttributePointGain = 1;
    [SerializeField] private float xpThresholdExponent;
    [SerializeField] private Species species;
    [SerializeField] private Ship ship;
    [SerializeField] private Sprite avatar;

    [Header("Data to update IN GAME")]
    [SerializeField] private PilotSaveData saveData;

    [Serializable]
    public class PilotSaveData
    {
        public string RandomName;
        public Species RandomSpecies;
        public int Level;
        public double RequiredXp;
        public double CurrentXp;
        public int MissionsCompleted;
        public bool IsHired;
        public PilotAttributes Attributes;
    }

    public const string FOLDER_NAME = "PilotSaveData";

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PilotSaveData>(name, FOLDER_NAME);
    }

    private void OnValidate()
    {
        // Cannot be below 1
        saveData.Level = Math.Max(saveData.Level, 1);
        saveData.Attributes.Navigation = Math.Max(saveData.Attributes.Navigation, 1);
        saveData.Attributes.Savviness = Math.Max(saveData.Attributes.Savviness, 1);

        // Cannot be below 0 
        saveData.RequiredXp = Math.Max(saveData.RequiredXp, 0);
        saveData.CurrentXp = Math.Max(saveData.CurrentXp, 0);
        saveData.MissionsCompleted = Math.Max(saveData.MissionsCompleted, 0);

        if (isRandom)
        {
            // Random pilot cannot have pre-declared name 
            pilotName = string.Empty;
        }
    }
}
