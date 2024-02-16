﻿using Events;
using System;
using System.Linq;
using UnityEngine;

public class PilotsManager : MonoBehaviour, IDataModelManager
{
    public static PilotsManager Instance { get; private set; }

    public PilotsContainer PilotsContainer;
    public Pilot[] Pilots => PilotsContainer.Elements;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (DataUtils.IsNewGame())
        {
            // Randomise pilots once required data has loaded 
            PilotAssetsManager.OnPilotTextDataLoaded += RandomisePilots;
        }
    }

    private void Start()
    {
        SingletonManager.EventService.Add<OnEndOfDayEvent>(OnEndOfDayHandler);
    }

    public void Init()
    {
        if (Pilots == null)
        {
            Debug.LogError("No pilot data");
        }
        HandleStartAndLeavingDates();
    }

    public static double AwardXp(Pilot pilot, double xpGained)
    {
        if (pilot != null)
        {
            pilot.CurrentXp += xpGained;

            if (pilot.CanLevelUp)
            {
                LevelUpPilot(pilot);
            }
        }

        return pilot.CurrentXp;
    }

    private static void LevelUpPilot(Pilot pilot)
    {
        pilot.Level++;
        pilot.RequiredXp = Math.Pow(pilot.RequiredXp, pilot.XpThresholdExponent);

        if (pilot.CanGainAttributePoint)
        {
            // TODO: Replace this choice with player input when the UI is done. 
            PilotAttributeType attributeType = PilotUtils.GetRandomAttributeType();

            GainAttributePoints(pilot, attributeType);
        }
    }

    public static void GainAttributePoints(Pilot pilot, PilotAttributeType attributeType, int value = 1)
    {
        if (pilot == null)
            return;

        switch (attributeType)
        {
            case PilotAttributeType.Navigation:
                pilot.Navigation += value;
                break;
            case PilotAttributeType.Savviness:
                pilot.Savviness += value;
                break;
            default:
                break;
        }
    }

    public void HirePilot(Pilot pilot)
    {
        if (pilot != null)
        {
            pilot.IsHired = true;
        }
    }

    public static Pilot[] HiredPilots => Instance.Pilots.Where(p => p != null && p.IsHired).ToArray();

    public static Pilot[] PilotsForHire => Instance.Pilots.Where(p => p != null && !p.IsHired && !p.HasLeft/* && !p.IsPendingStart*/).ToArray();

    public static Pilot[] PilotsInQueue => Instance.Pilots
            .Where(p => p.Ship.IsInQueue)
            .ToArray();

    public static bool PilotHasMission(Pilot pilot)
    {
        return MissionsManager.GetScheduledMission(pilot) != null;
    }

    public static bool PilotHasMissionInProgress(Pilot pilot)
    {
        ScheduledMission scheduled = MissionsManager.GetScheduledMission(pilot);
        return scheduled?.Mission != null && scheduled.Mission.IsInProgress();
    }

    public void RandomisePilots()
    {
        if (Pilots != null)
        {
            foreach (Pilot pilot in Pilots)
            {
                if (pilot == null)
                {
                    continue;
                }

                if (pilot.IsRandom)
                {
                    pilot.Species = PilotUtils.GetRandomSpecies();
                    pilot.Name = PilotAssetsManager.GetRandomName(pilot.Species);

                    RandomiseAvatar(pilot);
                }

                // We always randomise likes and dislikes 
                RandomisePreferences(pilot);
            }
        }

        // We only need to execute this callback once 
        PilotAssetsManager.OnPilotTextDataLoaded -= RandomisePilots;
    }

    private void RandomiseAvatar(Pilot pilot)
    {
        Sprite randomAvatar = PilotAssetsManager.GetRandomAvatar(pilot.Species);

        if (randomAvatar != null)
        {
            pilot.Avatar = randomAvatar;
        }
        else
        {
            Debug.Log($"Random avatar for {pilot.Species} (Species) was null");
        }
    }

    private void RandomisePreferences(Pilot pilot)
    {
        var preferences = PilotAssetsManager.GetRandomPreferences();

        if (!preferences.IsNullOrEmpty())
        {
            pilot.Like = preferences.like;
            pilot.Dislike = preferences.dislike;
        }
    }

    public static Pilot GetRandomHiredPilot() => HiredPilots.GetRandomElement();

    public static Pilot GetPilotByShip(Ship ship)
    {
        return Instance.Pilots.FirstOrDefault(pilot => pilot.Ship == ship);
    }

    private void OnEndOfDayHandler(OnEndOfDayEvent evt)
    {
        HandleStartAndLeavingDates();
    }

    private void HandleStartAndLeavingDates()
    {
        foreach (var pilot in Pilots)
        {
            if (CalendarManager.CurrentDate == new Date(1, 1, 1))
            {
                pilot.IsHired = pilot.StartsHired;
            }

            if (pilot.HasLeft && pilot.IsHired)
            {
                pilot.IsHired = false;
                Debug.Log($"Current date {CalendarManager.CurrentDate} has reached {pilot}'s leaving date. Pilot no longer hired.");
                continue;
            }
            //// Note: If we implement selling pilots, this will need to be changed 
            //if (pilot.HasStarted && !pilot.IsHired)
            //{
            //    pilot.IsHired = true;
            //    Debug.Log($"Current date {CalendarManager.CurrentDate} has reached {pilot}'s start date. Pilot now hired.");
            //}
        }
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Pilot pilot in Instance.Pilots)
        {
            pilot.SaveData();
        }
    }

    public async void LoadDataAsync()
    {
        if (!DataUtils.SaveFolderExists(Pilot.FolderName))
        {
            DataUtils.CreateSaveFolder(Pilot.FolderName);
            return;
        }

        foreach (Pilot pilot in Instance.Pilots)
        {
            await pilot.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Pilot.FolderName);
    }

    public void ResetData()
    {
        foreach (var pilot in Instance.Pilots)
        {
            pilot.ResetData();
        }
    }
    #endregion
}
