using System.Threading.Tasks;

[System.Serializable]
public class ScheduledMission
{
    public Mission Mission { get; set; }
    public Pilot Pilot { get; set; }

    //public ScheduledMissionSaveData saveData;
    //public string FileName => $"{saveData.mission.name}_{saveData.pilot.name}";

    //public static string FOLDER_NAME = "ScheduledMissionSaveData";

    //[System.Serializable]
    //public class ScheduledMissionSaveData
    //{
    //    public Mission mission;
    //    public Pilot pilot;
    //}

    public ScheduledMission()
    {

    }

    //public ScheduledMission()
    //{
    //    // Paramless and then call load afterwards 
    //    // Or if creating for first time, just set fields inline

    //    // This would avoid the loading problem somewhat
    ////}

    //public async Task LoadDataAsync()
    //{
    //    saveData = await DataUtils.LoadFileAsync<ScheduledMissionSaveData>(FileName, FOLDER_NAME);
    //}

    //public void SaveData()
    //{
    //    DataUtils.SaveFileAsync(FileName, FOLDER_NAME, saveData);
    //}

    //public Mission Mission { get => saveData.mission; set => saveData.mission = value; }
    //public Pilot Pilot { get => saveData.pilot; set => saveData.pilot = value; }
}
