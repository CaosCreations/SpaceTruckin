using System;
using System.Linq;
using UnityEngine;

public class PilotsManager : MonoBehaviour, IDataModelManager
{   
    public static PilotsManager Instance { get; private set; }

    public PilotsContainer pilotsContainer;
    public Pilot[] Pilots { get => pilotsContainer.pilots; }

    void Awake()
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
    }

    public void Init()
    {
        if (Pilots == null)
        {
            Debug.LogError("No pilot data");
        }

        if (DataUtils.SaveFolderExists(Pilot.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Pilot.FOLDER_NAME);
        }
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
    }

    public void HirePilot(Pilot pilot)
    {
        if (pilot != null)
        {
            pilot.IsHired = true;
        }
    }

    public static Pilot[] HiredPilots => Instance.Pilots.Where(p => p.IsHired).ToArray();

    public static Pilot[] PilotsForHire => Instance.Pilots.Where(p => !p.IsHired).ToArray();

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
                else if (pilot.IsRandom && string.IsNullOrEmpty(pilot.Name))
                {
                    pilot.Species = PilotUtils.GetRandomSpecies();
                    pilot.Name = PilotTextManager.Instance.GetRandomName(pilot.Species);

                    // Other random stats logic here
                }

                // We always randomise likes and dislikes 
                var preferences = PilotTextManager.GetRandomPreferences();
                
                if (!preferences.IsNullOrEmpty())
                {
                    pilot.Like = preferences.Item1;
                    pilot.Dislike = preferences.Item2;
                }
            }
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
        foreach (Pilot pilot in Instance.Pilots)
        {
            await pilot.LoadDataAsync();
        }
    }

    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Pilot.FOLDER_NAME);
    }
    #endregion
}
