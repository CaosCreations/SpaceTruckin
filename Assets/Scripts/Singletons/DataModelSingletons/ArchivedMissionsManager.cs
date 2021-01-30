using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArchivedMissionsManager : MonoBehaviour, IDataModelManager
{
    public static ArchivedMissionsManager Instance { get; private set; }
    public List<ArchivedMission> ArchivedMissions { get; set; } // field vs. prop auto-imp

    /// <summary>
    /// Missions that were completed yesterday.
    /// These are ready to displayed in the daily mission report.
    /// </summary>
    public List<ArchivedMission> MissionsCompletedYesterday { get; set; }
    //
    // but these will be overwritten if the player doesn't check the terminal immediately.
    // is that a problem?
    // i.e. do they need to remain in the terminal and build up if the player doesn't check?

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
        //Init();
    }

    public void Init()
    {
        if (DataModelsUtils.SaveFolderExists(ArchivedMission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataModelsUtils.CreateSaveFolder(ArchivedMission.FOLDER_NAME);
        }
        ArchivedMissions = new List<ArchivedMission>();
        MissionsCompletedYesterday = new List<ArchivedMission>();
    }

    public static bool WereMissionsCompletedYesterday()
    {
        return Instance.MissionsCompletedYesterday != null
            && Instance.MissionsCompletedYesterday.Any();
    }

    public static void AddToArchive(ArchivedMission archivedMission)
    {
        Instance.MissionsCompletedYesterday.Add(archivedMission);
        Instance.ArchivedMissions.Add(archivedMission);
    }

    public async void LoadDataAsync()
    {
        ArchivedMissions = new List<ArchivedMission>();

        if (MissionsManager.Instance.Missions != null)
        {
            foreach (Mission mission in MissionsManager.Instance.Missions
                .Where(m => m.NumberOfCompletions > 0))
            {
                // A mission has been completed n times, 
                // so we create n archived missions.
                for (int i = 0; i < mission.NumberOfCompletions; i++)
                {
                    // We use the loop counter to construct the file name, 
                    // which is determined by the number of completions
                    // that mission had at the time it was completed.
                    ArchivedMission newArchivedMission = new ArchivedMission(
                        mission, completionNumber: i + 1);
                    
                    await newArchivedMission.LoadDataAsync();
                    ArchivedMissions.Add(newArchivedMission);
                }
            }
        }
    }

    public void SaveData() => ArchivedMissions.ForEach(a => a.SaveData());

    public void DeleteData() =>
        DataModelsUtils.RecursivelyDeleteSaveData(ArchivedMission.FOLDER_NAME);
}
