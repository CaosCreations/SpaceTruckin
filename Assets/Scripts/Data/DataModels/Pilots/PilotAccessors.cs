using UnityEngine;

public partial class Pilot
{
    // This class is just for property accessors. 
    // The fields are all located in Pilot.cs. 

    public string Name
    {
        get => IsRandom ? saveData.randomName : pilotName;
        set
        {
            if (IsRandom)
            {
                saveData.randomName = value;
            }
            else
            {
                pilotName = value;
            }
        }
    }
    public Species Species
    {
        get => isRandom ? saveData.randomSpecies : species;
        set
        {
            if (isRandom)
            {
                saveData.randomSpecies = value;
            }
            else
            {
                species = value;
            }
        }
    }
    public string Like { get => like; set => like = value; }
    public string Dislike { get => dislike; set => dislike = value; }
    public int HireCost { get => hireCost; }
    public double CurrentXp { get => saveData.currentXp; set => saveData.currentXp = value; }
    public double RequiredXp { get => saveData.requiredXp; set => saveData.requiredXp = value; }
    public bool CanLevelUp { get => CurrentXp >= RequiredXp; }
    public float XpThresholdExponent
    {
        get => xpThresholdExponent; set => xpThresholdExponent = value;
    }
    public int Level { get => saveData.level; set => saveData.level = value; }
    public int MissionsCompleted
    {
        get => saveData.missionsCompleted; set { saveData.missionsCompleted = value; }
    }
    public bool IsHired
    {
        get => saveData.isHired; set { saveData.isHired = value; }
    }
    public bool HasMission => PilotsManager.PilotHasMission(this);
    public bool IsOnMission => PilotsManager.PilotHasMissionInProgress(this);
    public Mission CurrentMission => MissionsManager.GetMission(this);
    public bool IsRandom => isRandom;
    public Ship Ship { get => ship; }
    public Sprite Avatar { get => avatar; set => avatar = value; }
}
