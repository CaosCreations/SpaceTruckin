﻿using UnityEngine;

public partial class Pilot
{
    // This class is just for property accessors. 
    // The fields are all located in Pilot.cs. 

    public string Name
    {
        get => IsRandom ? saveData.RandomName : pilotName;
        set
        {
            if (IsRandom)
            {
                saveData.RandomName = value;
            }
            else
            {
                pilotName = value;
            }
        }
    }
    public Species Species
    {
        get => isRandom ? saveData.RandomSpecies : species;
        set
        {
            if (isRandom)
            {
                saveData.RandomSpecies = value;
            }
            else
            {
                species = value;
            }
        }
    }
    public string Like { get => like; set => like = value; }
    public string Dislike { get => dislike; set => dislike = value; }
    public int HireCost => hireCost;
    public double CurrentXp { get => saveData.CurrentXp; set => saveData.CurrentXp = value; }
    public double RequiredXp { get => saveData.RequiredXp; set => saveData.RequiredXp = value; }
    public bool CanLevelUp => CurrentXp >= RequiredXp;
    public float XpThresholdExponent
    {
        get => xpThresholdExponent; set => xpThresholdExponent = value;
    }
    public int Level { get => saveData.Level; set => saveData.Level = value; }
    public int MissionsCompleted
    {
        get => saveData.MissionsCompleted; set { saveData.MissionsCompleted = value; }
    }
    public bool IsHired
    {
        get => saveData.IsHired; set { saveData.IsHired = value; }
    }
    public bool HasMission => PilotsManager.PilotHasMission(this);
    public bool IsOnMission => PilotsManager.PilotHasMissionInProgress(this);
    public Mission CurrentMission => MissionsManager.GetMission(this);
    public bool IsRandom => isRandom;
    public Ship Ship => ship;
    public Sprite Avatar { get => avatar; set => avatar = value; }
    public int Navigation
    {
        get => saveData.Attributes.Navigation; set => saveData.Attributes.Navigation = value;
    }
    public int Savviness
    {
        get => saveData.Attributes.Savviness; set => saveData.Attributes.Savviness = value;
    }
    public PilotAttributes Attributes => saveData.Attributes;
    public int LevelsNeededForAttributePointGain => levelsNeededForAttributePointGain;
    public bool CanGainAttributePoint => Level % LevelsNeededForAttributePointGain == 0;
    public PilotTrait[] Traits => traits;
    public PilotSaveData PilotData => saveData;
}
