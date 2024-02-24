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

[CreateAssetMenu(fileName = "Pilot", menuName = "ScriptableObjects/Pilots/Pilot", order = 1)]
public partial class Pilot : ScriptableObject, IDataModel
{
    [Header("Set in Editor")]
    [SerializeField] private bool isRandom;
    [SerializeField] private string pilotName;
    [SerializeField] private string like;
    [SerializeField] private string dislike;
    [SerializeField] private int hireCost;
    [SerializeField] private bool startsHired;
    [SerializeField] private Date startDate;
    [SerializeField] private Date leavingDate;
    [SerializeField] private int levelsNeededForAttributePointGain = 1;
    [SerializeField] private float xpThresholdExponent;
    [SerializeField] private Species species;
    [SerializeField] private Ship ship;
    [SerializeField] private Sprite avatar;
    [SerializeField] private PilotTrait[] traits;

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

        public PilotSaveData()
        {
            OnValidate();
        }

        public void OnValidate()
        {
            // Cannot be below 1
            Level = Math.Max(Level, 1);
            Attributes.Navigation = Math.Max(Attributes.Navigation, 1);
            Attributes.Savviness = Math.Max(Attributes.Savviness, 1);

            // Cannot be below 0 
            RequiredXp = Math.Max(RequiredXp, 0);
            CurrentXp = Math.Max(CurrentXp, 0);
            MissionsCompleted = Math.Max(MissionsCompleted, 0);

            // Cannot be below custom threshold 
            RequiredXp = Math.Max(RequiredXp, PilotsConstants.MinimumRequiredXp);
        }
    }

    public const string FolderName = "PilotSaveData";

    public void SaveData()
    {
        DataUtils.SaveFileAsync(name, FolderName, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<PilotSaveData>(name, FolderName);
    }

    public int GetAttributeLevelByType(PilotAttributeType attributeType)
    {
        return attributeType == PilotAttributeType.Navigation ? Navigation : Savviness;
    }

    private void OnValidate()
    {
        saveData.OnValidate();

        if (isRandom)
        {
            // Random pilot cannot have pre-declared name 
            pilotName = string.Empty;
        }
    }

    public override string ToString()
    {
        return $"Pilot \"{name}\"";
    }

    public void ResetData()
    {
        saveData = new();
    }
}
