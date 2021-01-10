using System;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardItem : MonoBehaviour
{
    public Mission mission;
    public Text missionNameText;
    public Button missionInfoButton;

    public void Init(Mission mission, Action<Mission> func)
    {
        this.mission = mission;
        missionNameText.text = mission.MissionName;

        missionInfoButton.onClick.RemoveAllListeners();
        missionInfoButton.onClick.AddListener(() => func(mission));
    }
}
