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
        if (DataModelsUtils.SaveFolderExists(Mission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(Mission.FOLDER_NAME);
        }

        if (Missions == null)
        {
            Debug.LogError("No mission data found");
        }
    }

    public static List<Mission> GetAcceptedMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Ship == null)
            .ToList();
    }

    public static List<Mission> GetScheduledMissions()
    {
        return Instance.Missions
            .Where(x => x.HasBeenAccepted
            && x.Ship != null)
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

        if (mission.Outcomes == null)
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
        DataModelsUtils.RecursivelyDeleteSaveData(Mission.FOLDER_NAME);
    }
}
