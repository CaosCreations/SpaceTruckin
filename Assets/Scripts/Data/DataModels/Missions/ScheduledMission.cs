﻿using System;
using UnityEngine;

[Serializable]
public class ScheduledMission
{
    [SerializeField] private Mission mission;
    [SerializeField] private Pilot pilot;

    public Mission Mission { get => mission; set => mission = value; }
    public Pilot Pilot { get => pilot; set => pilot = value; }

    public const string FILE_NAME = "ScheduledMissionSaveData"; // We store them in all in one file
    public static string FilePath => DataUtils.GetSaveFilePath(Mission.FOLDER_NAME, FILE_NAME);

    public override string ToString()
    {
        if (mission != null && pilot != null)
        {
            // Return the mission and pilot pairing as a string 
            return $"{mission.Name} (Mission), {pilot.Name} (Pilot)";
        }

        return string.Empty;
    }
}
