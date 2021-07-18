using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArchivedMissionsManager : MonoBehaviour, IDataModelManager
{
    public static ArchivedMissionsManager Instance { get; private set; }
    public List<ArchivedMission> ArchivedMissions { get; set; }
    public List<ArchivedMission> MissionsCompletedYesterday { get; set; }

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
        if (DataUtils.SaveFolderExists(ArchivedMission.FOLDER_NAME))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(ArchivedMission.FOLDER_NAME);
        }

        ArchivedMissions = new List<ArchivedMission>();
        MissionsCompletedYesterday = new List<ArchivedMission>();

        if (ArchivedMissions == null)
        {
            Debug.LogError("No archived mission data");
        }
    }

    public static bool WereMissionsCompletedYesterday()
    {
        return Instance.MissionsCompletedYesterday != null
            && Instance.MissionsCompletedYesterday.Any();
    }

    public static void ResetMissionsCompletedYesterday()
        => Instance.MissionsCompletedYesterday.Clear();

    public static void AddToArchive(ArchivedMission archivedMission)
    {
        Instance.MissionsCompletedYesterday.Add(archivedMission);
        Instance.ArchivedMissions.Add(archivedMission);
    }

    public static bool ThereAreMissionsToReport()
    {
        foreach (var archivedMission in Instance.ArchivedMissions)
        {
            if (archivedMission != null && !archivedMission.HasBeenViewedInReport)
            {
                return true;
            }
        }
        return false;
    }

    public static List<ArchivedMission> GetMissionsToAppearInReport()
    {
        var missionsToAppearInReport = new List<ArchivedMission>();

        foreach (var archivedMission in Instance.ArchivedMissions)
        {
            if (archivedMission != null && !archivedMission.HasBeenViewedInReport)
            {
                missionsToAppearInReport.Add(archivedMission);
            }
        }
        return missionsToAppearInReport;
    }

    #region Persistence
    public async void LoadDataAsync()
    {
        string json = await DataUtils.ReadFileAsync(ArchivedMission.FilePath);
        ArchivedMissions = JsonHelper.ListFromJson<ArchivedMission>(json);
    }

    public void SaveData()
    {
        string json = JsonHelper.ListToJson(ArchivedMissions);
        string folderPath = DataUtils.GetSaveFolderPath(ArchivedMission.FOLDER_NAME);
        DataUtils.SaveFileAsync(ArchivedMission.FILE_NAME, folderPath, json);
    }

    public void DeleteData() 
        => DataUtils.RecursivelyDeleteSaveData(ArchivedMission.FOLDER_NAME);
    #endregion
}
