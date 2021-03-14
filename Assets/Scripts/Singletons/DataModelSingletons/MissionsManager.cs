using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionsManager : MonoBehaviour, IDataModelManager
{
    public static MissionsManager Instance { get; private set; }

    [SerializeField] private MissionContainer missionContainer;
    [SerializeField] private MissionOutcomeContainer missionOutcomeContainer;
    public Mission[] Missions { get => missionContainer.missions; }
    public MissionOutcome[] Outcomes { get => missionOutcomeContainer.missionOutcomes; }

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
    }

    public void Init()
    {
        if (DataUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(Mission.FOLDER_NAME);
        }

        if (Missions == null)
        {
            Debug.LogError("No mission data found");
        }
    }

    /// <summary>
    /// Missions are 'accepted' if they have been taken from the notice board
    /// but a pilot is not currently assigned to them
    /// </summary>
    /// <returns></returns>
    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Pilot == null)
            .ToList();
    }

    /// <summary>
    /// Missions are 'scheduled' if a pilot has been assigned to them 
    /// but that pilot has not yet left the hangar 
    /// </summary>
    public static List<Mission> GetScheduledMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Pilot != null
            && !x.IsInProgress())
            .ToList();
    }

    /// <summary>
    /// Missions are 'in progress' if the pilot assigned to them has left the hangar
    /// </summary>
    public static List<Mission> GetMissionsInProgress()
    {
        return Instance.Missions
            .Where(x => x.IsInProgress())
            .ToList();
    }

    public static Mission GetMissionByFileName(string fileName)
    {
        return Instance.Missions.FirstOrDefault(x => x.name == fileName);
    }

    public static void UpdateMissionSchedule()
    {
        // Reset yesterday's missions, so today's will take their place. 
        ArchivedMissionsManager.ResetMissionsCompletedYesterday();

        foreach (Mission mission in Instance.Missions)
        {
            if(mission.IsInProgress())
            {
                mission.DaysLeftToComplete--;

                // We just finished the mission
                if (!mission.IsInProgress())
                {
                    CompleteMission(mission);
                }
            }
        }
    }

    private static void CompleteMission(Mission mission)
    {
        // Send a thank you email on first completion of the mission.
        if (mission.ThankYouMessage != null && mission.NumberOfCompletions <= 0)
        {
            mission.ThankYouMessage.IsUnlocked = true;
        }
        mission.NumberOfCompletions++;

        // Instantiate an archived mission object to store the stats of the completed mission.
        mission.MissionToArchive = new ArchivedMission(mission, mission.NumberOfCompletions);

        mission.Pilot.MissionsCompleted++;
        mission.MissionToArchive.MissionsCompletedByPilotAtTimeOfMission = mission.Pilot.MissionsCompleted;

        // Randomise the mission's outcomes if flag is set or they are missing. 
        if (mission.HasRandomOutcomes)
        {
            AssignRandomOutcomes(mission);
        }

        // We will set the archived mission fields throughout the outcome processing. 
        mission.ProcessOutcomes();

        // Add the object to the archive once all outcomes have been processed. 
        ArchivedMissionsManager.AddToArchive(mission.MissionToArchive);

        ShipsManager.DockShip(mission.Ship);
    }

    private static void AssignRandomOutcomes(Mission mission)
    {
        var randomOutcomes = new List<MissionOutcome>();
        Instance.Outcomes.Shuffle();

        MoneyOutcome moneyOutcome = MissionUtils.GetOutcomeByType<MoneyOutcome>(Instance.Outcomes);
        PilotXpOutcome pilotXpOutcome = MissionUtils.GetOutcomeByType<PilotXpOutcome>(Instance.Outcomes);
        OmenOutcome omenOutcome = MissionUtils.GetOutcomeByType<OmenOutcome>(Instance.Outcomes);
        ShipDamageOutcome shipDamageOutcome = MissionUtils.GetOutcomeByType<ShipDamageOutcome>(Instance.Outcomes);

        if (moneyOutcome != null) randomOutcomes.Add(moneyOutcome);
        if (pilotXpOutcome != null) randomOutcomes.Add(pilotXpOutcome);
        if (omenOutcome != null) randomOutcomes.Add(omenOutcome);
        if (shipDamageOutcome != null) randomOutcomes.Add(shipDamageOutcome);

        mission.Outcomes = randomOutcomes.ToArray();
    }

    #region Persistence
    public void SaveData()
    {
        foreach (Mission mission in Instance.Missions)
        {
            mission.SaveData();
        }
    }

    public async void LoadDataAsync()
    {
        foreach (Mission mission in Instance.Missions)
        {
            await mission.LoadDataAsync();
        }
    }
    
    public void DeleteData()
    {
        DataUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
    #endregion
}
