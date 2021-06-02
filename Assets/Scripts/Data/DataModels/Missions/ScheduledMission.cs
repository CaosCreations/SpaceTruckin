using UnityEngine;

[System.Serializable]
public class ScheduledMission
{
    [SerializeField] private Mission mission;
    [SerializeField] private Pilot pilot;

    public Mission Mission { get => mission; set => mission = value; }
    public Pilot Pilot { get => pilot; set => pilot = value; }
    public const string FILE_NAME = "ScheduledMissionSaveData"; // We store them in all in one file
    public static string FILE_PATH
    {
        get => DataUtils.GetSaveFilePath(Mission.FOLDER_NAME, FILE_NAME);
    }
}
