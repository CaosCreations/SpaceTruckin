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
    [SerializeField] private bool isRandom;
    [SerializeField] private string pilotName;
    [SerializeField] private string like;
    [SerializeField] private string dislike;
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
        public string RandomName;
        public Species RandomSpecies;
        public int Level;
        public double RequiredXp;
        public double CurrentXp;
        public int MissionsCompleted;
        public bool IsHired;
    }

    public const string FOLDER_NAME = "PilotSaveData";

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async System.Threading.Tasks.Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PilotSaveData>(name, FOLDER_NAME);
    }

    private void OnValidate()
    {
        // Cannot be below 1
        saveData.Level = Math.Max(saveData.Level, 1);
        saveData.RequiredXp = Math.Max(saveData.RequiredXp, 1);
        saveData.CurrentXp = Math.Max(saveData.CurrentXp, 1);
        saveData.MissionsCompleted = Math.Max(saveData.MissionsCompleted, 1);

        if (isRandom)
        {
            // Random pilot cannot have pre-declared name 
            pilotName = string.Empty;
        }
    }
}
