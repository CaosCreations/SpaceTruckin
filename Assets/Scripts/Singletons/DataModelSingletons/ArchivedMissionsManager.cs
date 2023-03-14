using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArchivedMissionsManager : MonoBehaviour, IDataModelManager
{
    public static ArchivedMissionsManager Instance { get; private set; }
    public List<ArchivedMission> ArchivedMissions { get; set; }

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
        ArchivedMissions = new List<ArchivedMission>();
    }

    public static void AddToArchive(ArchivedMission archivedMission)
    {
        Instance.ArchivedMissions.Add(archivedMission);
    }

    public static List<ArchivedMission> GetMissionsCompletedYesterday()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null && x.CompletionDate.ToDays() == CalendarManager.CurrentDate.ToDays() - 1)
            .ToList();
    }

    public static bool ThereAreMissionsToReport()
    {
        return Instance.ArchivedMissions.Any(x => x != null && !x.HasBeenViewedInReport);
    }

    public static List<ArchivedMission> GetMissionsToAppearInReport()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null && !x.HasBeenViewedInReport)
            .ToList();
    }

    public static ArchivedMission GetMostRecentMissionByPilot(Pilot pilot)
    {
        return Instance.ArchivedMissions
            .OrderByDescending(x => x?.CompletionDate)
            .FirstOrDefault(x => x?.Pilot == pilot);
    }

    public static IEnumerable<ArchivedMission> GetArchivedMissionsByPilot(Pilot pilot)
    {
        return Instance.ArchivedMissions.Where(x => x != null && x.Pilot == pilot);
    }

    #region Persistence
    public async void LoadDataAsync()
    {
        if (!DataUtils.SaveFolderExists(ArchivedMission.FolderName))
        {
            DataUtils.CreateSaveFolder(ArchivedMission.FolderName);
            return;
        }

        string json = await DataUtils.ReadFileAsync(ArchivedMission.FilePath);
        ArchivedMissions = JsonHelper.ListFromJson<ArchivedMission>(json);
    }

    public void SaveData()
    {
        string json = JsonHelper.ListToJson(ArchivedMissions);
        string folderPath = DataUtils.GetSaveFolderPath(ArchivedMission.FolderName);
        DataUtils.SaveFileAsync(ArchivedMission.FileName, folderPath, json);
    }

    public void DeleteData() => DataUtils.RecursivelyDeleteSaveData(ArchivedMission.FolderName);
    #endregion
}
