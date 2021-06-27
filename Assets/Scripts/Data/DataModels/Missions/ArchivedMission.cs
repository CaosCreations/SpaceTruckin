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

    public ArchivedMission(Mission mission, Pilot pilot, int completionNumber)
    {
        saveData = new ArchivedMissionSaveData();
        MissionName = mission.Name;
        CompletionNumber = completionNumber;
        FileName = $"{mission.name}_{completionNumber}";
        Pilot = pilot;
        TotalFuelLost = mission.FuelCost;
        CompletionDate = CalendarManager.Instance.CurrentDate;
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
        public int completionNumber, totalDamageTaken, totalDamageReduced, totalFuelLost;
        public long totalMoneyEarned, totalMoneyIncrease;
        public double totalPilotXpGained, totalXpIncreaseFromLicences;
        public Pilot pilot;
        public int pilotLevelAtTimeOfMission;
        public int missionsCompletedByPilotAtTimeOfMission;
        public Date completionDate;
    }

    public async Task LoadDataAsync()
    {
        saveData = await DataUtils.LoadFileAsync<ArchivedMissionSaveData>(FileName, FOLDER_NAME);
    }

    public void SaveData()
    {
        DataUtils.SaveFileAsync(FileName, FOLDER_NAME, saveData);
    }

    public string MissionName { get => saveData.missionName; set => saveData.missionName = value; }
    public int CompletionNumber { get => saveData.completionNumber; set => saveData.completionNumber = value; }
    public long TotalMoneyEarned { get => saveData.totalMoneyEarned; set => saveData.totalMoneyEarned = value; }
    public long TotalMoneyIncrease { get => saveData.totalMoneyIncrease; set => saveData.totalMoneyIncrease = value; }
    public int TotalDamageTaken { get => saveData.totalDamageTaken; set => saveData.totalDamageTaken = value; }
    public int TotalDamageReduced { get => saveData.totalDamageReduced; set => saveData.totalDamageReduced = value; }
    public int PilotLevelAtTimeOfMission
    {
        get => saveData.pilotLevelAtTimeOfMission; set => saveData.pilotLevelAtTimeOfMission = value;
    }
    public int MissionsCompletedByPilotAtTimeOfMission
    {
        get => saveData.missionsCompletedByPilotAtTimeOfMission;
        set => saveData.missionsCompletedByPilotAtTimeOfMission = value;
    }
    public double TotalPilotXpGained { get => saveData.totalPilotXpGained; set => saveData.totalPilotXpGained = value; }
    public double TotalXpIncreaseFromLicences
    {
        get => saveData.totalXpIncreaseFromLicences;
        set => saveData.totalXpIncreaseFromLicences = value;
    }
    public int TotalFuelLost { get => saveData.totalFuelLost; set => saveData.totalFuelLost = value; }
    public Pilot Pilot { get => saveData.pilot; set => saveData.pilot = value; }
    public Ship Ship => Pilot.Ship;
    public Date CompletionDate { get => saveData.completionDate; set => saveData.completionDate = value; }
}
