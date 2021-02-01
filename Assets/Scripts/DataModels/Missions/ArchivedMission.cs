using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This model holds data about past missions.
/// The fields of this class will be set 
/// during the processing of the mission outcomes.
/// </summary>
public class ArchivedMission : IDataModel
{
    /// <summary>
    /// We set the file name based on the mission's name and its completion number.
    /// e.g. 'Smash_69' would be the 69th completion of the mission 'Smash.' 
    /// </summary>
    public string FileName { get; private set; }
    public static string FOLDER_NAME { get; } = "ArchivedMissionSaveData";

    [HideInInspector]
    private ArchivedMissionSaveData saveData;

    public ArchivedMission(Mission mission, int completionNumber)
    {
        saveData = new ArchivedMissionSaveData();
        MissionName = mission.Name;
        CompletionNumber = completionNumber;
        FileName = $"{MissionName}_{completionNumber}";
        Ship = mission.Ship;
        TotalFuelLost = mission.FuelCost;
    }

    /// <summary>
    /// We are only interested in storing the totals.
    /// We can look up the rest of the mission fields 
    /// by the mission's name if needed.
    /// </summary>
    [System.Serializable]
    public class ArchivedMissionSaveData
    {
        public string missionName;
        public int completionNumber, totalDamageTaken, totalFuelLost;
        public long totalMoneyEarned;
        public double totalPilotXpGained;
        public Ship ship;

        /// <summary>
        /// The level of the pilot at the point in time that this archived mission was completed.
        /// </summary>
        public int contemporaneousPilotLevel; 
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataModelsUtils.LoadFileAsync<ArchivedMissionSaveData>(FileName, FOLDER_NAME);
    }

    public void SaveData()
    {
        DataModelsUtils.SaveFileAsync(FileName, FOLDER_NAME, saveData);
    }

    public string MissionName { get => saveData.missionName; set => saveData.missionName = value; }
    public int CompletionNumber { get => saveData.completionNumber; set => saveData.completionNumber = value; }
    public long TotalMoneyEarned { get => saveData.totalMoneyEarned; set => saveData.totalMoneyEarned = value; }
    public int TotalDamageTaken { get => saveData.totalDamageTaken; set => saveData.totalDamageTaken = value; }
    public int ContemporaneousPilotLevel 
    {
        get => saveData.contemporaneousPilotLevel; set => saveData.contemporaneousPilotLevel = value; 
    }
    public double TotalPilotXpGained { get => saveData.totalPilotXpGained; set => saveData.totalPilotXpGained = value; }
    public int TotalFuelLost { get => saveData.totalFuelLost; set => saveData.totalFuelLost = value; }
    public Ship Ship { get => saveData.ship; set => saveData.ship = value; }
    public Pilot Pilot => Ship.Pilot; 
}
