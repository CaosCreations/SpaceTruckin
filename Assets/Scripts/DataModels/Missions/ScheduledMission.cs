using System.Threading.Tasks;

public class ScheduledMission : IDataModel
{
    public Mission mission;
    public Pilot pilot;
    public static string FOLDER_NAME = "ScheduledMissionSaveData";

    public Task LoadDataAsync()
    {
        throw new System.NotImplementedException();
    }

    public void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
