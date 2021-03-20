[System.Serializable]
public class ScheduledMission
{
    private Mission mission;
    private Pilot pilot;
    public Mission Mission { get => mission; set => mission = value; }
    public Pilot Pilot { get => pilot; set => pilot = value; }
    public static string FILE_NAME => "ScheduledMissionSaveData";
    public static string FILE_PATH
    {
        get => DataUtils.GetSaveFilePath(Mission.FOLDER_NAME, FILE_NAME);
    }
}
