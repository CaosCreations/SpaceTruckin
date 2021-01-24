using System;
using System.Threading.Tasks;
using UnityEngine;

public interface IMissionOutcome
{
    void Process(Mission mission);
}

public class MissionOutcome : ScriptableObject, IMissionOutcome, IDataModel
{
    public int probability;
    public string flavourText;

    public MissionOutcomeSaveData saveData;
    public static string FOLDER_NAME = "MissionOutcomeSaveData";

    [Serializable]
    public class MissionOutcomeSaveData
    {
        // Data common to all types of outcomes. 
        // e.g. Date completed. 
    }

    public virtual void Process(Mission mission) { }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(name, FOLDER_NAME, saveData);
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<MissionOutcomeSaveData>(name, FOLDER_NAME);
    }
}