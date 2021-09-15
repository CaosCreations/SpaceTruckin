﻿using System.Collections.Generic;
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
        if (DataUtils.SaveFolderExists(ArchivedMission.FolderName))
        {
            LoadDataAsync();
        }
        else
        {
            DataUtils.CreateSaveFolder(ArchivedMission.FolderName);
        }

        ArchivedMissions = new List<ArchivedMission>();

        if (ArchivedMissions == null)
        {
            Debug.LogError("No archived mission data");
        }
    }

    public static void AddToArchive(ArchivedMission archivedMission)
    {
        Instance.ArchivedMissions.Add(archivedMission);
    }

    public static List<ArchivedMission> GetMissionsCompletedYesterday()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null
            && x.CompletionDate.ToDays() == CalendarManager.Instance.CurrentDate.ToDays() - 1)
            .ToList();
    }

    public static bool ThereAreMissionsToReport()
    {
        return Instance.ArchivedMissions
            .Any(x => x != null && !x.HasBeenViewedInReport);
    }

    public static List<ArchivedMission> GetMissionsToAppearInReport()
    {
        return Instance.ArchivedMissions
            .Where(x => x != null && !x.HasBeenViewedInReport)
            .ToList();
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
        string folderPath = DataUtils.GetSaveFolderPath(ArchivedMission.FolderName);
        DataUtils.SaveFileAsync(ArchivedMission.FileName, folderPath, json);
    }

    public void DeleteData()
        => DataUtils.RecursivelyDeleteSaveData(ArchivedMission.FolderName);
    #endregion
}
