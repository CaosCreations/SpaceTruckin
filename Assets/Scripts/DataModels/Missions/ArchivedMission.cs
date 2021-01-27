using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This holds data about mission gains/losses.
/// The fields of this class/subclass will be set 
/// during the processing of the mission outcomes.
/// </summary>
public class ArchivedMission : IDataModel
{
    private string fileName;

    public ArchivedMission(string fileName)
    {
        this.fileName = fileName; // pass in missionname and completionnumber 
    }

    [HideInInspector]
    private ArchivedMissionSaveData saveData;

    public static string FOLDER_NAME = "ArchivedMissionSaveData";
    public string FileName { get => fileName; set => fileName = value; }

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
