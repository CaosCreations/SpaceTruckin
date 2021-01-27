using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This model holds data about mission statistics.
/// The fields of this class/subclass will be set 
/// during the processing of the mission outcomes.
/// </summary>
public class ArchivedMission : IDataModel
{
    /// <summary>
    /// We build the file name based on the mission's name and its completion number.
    /// e.g. 'Smash_69' would be the 69th completion of the mission 'Smash' 
    /// </summary>
    private string fileName;
    public string FileName { get => fileName; set => fileName = value; }

    public ArchivedMission(string fileName)
    {
        this.fileName = fileName; // pass in missionname and completionnumber 
    }

    [HideInInspector]
    private ArchivedMissionSaveData saveData;

    public static string FOLDER_NAME = "ArchivedMissionSaveData";

    public async Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<ArchivedMissionSaveData>(FileName, FOLDER_NAME);
    }

    public void SaveData()
    {
        //string fileName = $"MissionName_{CompletionNumber}";
        DataModelsUtils.SaveFileAsync(FileName, FOLDER_NAME, saveData);
    }

    public class ArchivedMissionSaveData
    {
        public string missionName;
        public int completionNumber; 
        public long totalMoneyEarned;
        public int totalDamageTaken;
        public int totalFuelLost;
        public int totalPilotXpGained;
        public Ship shipUsed; // might need to be id (not instance id)
    }

    public string MissionName { get => saveData.missionName; set => saveData.missionName = value; }
    public int CompletionNumber { get => saveData.completionNumber; set => saveData.completionNumber = value; }
    public long TotalMoneyEarned { get => saveData.totalMoneyEarned; set => saveData.totalMoneyEarned = value; }
    public int TotalDamageTaken { get => saveData.totalDamageTaken; set => saveData.totalDamageTaken = value; }
    public int TotalPilotXpGained { get => saveData.totalPilotXpGained; set => saveData.totalPilotXpGained = value; }
    public int TotalFuelLost { get => saveData.totalFuelLost; set => saveData.totalFuelLost = value; }
    public Ship ShipUsed { get => saveData.shipUsed; set => saveData.shipUsed = value; }

}
