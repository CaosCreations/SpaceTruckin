﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoard : MonoBehaviour
{
    [Header("Set in prefab")]
    // Left Panel
    public GameObject scrollViewContent;
    public GameObject scrollItemPrefab;

    // Right Panel
    public Text jobNameText;
    public Text customerNameText;
    public Text cargoText;
    public Text rewardText;
    public Button acceptJobButton;

    [Header("Set in game")]
    public Mission selectedMission;

    void Start()
    {
        acceptJobButton.onClick.RemoveAllListeners();
        acceptJobButton.onClick.AddListener(AcceptMission);
    }

    private void OnEnable()
    {
        selectedMission = null;
        CleanDetailPane();
        CleanScrollView();
        PopulateScrollView();
    }

    void AcceptMission()
    {
        if(selectedMission != null)
        {
            selectedMission.hasBeenAccepted = true;
            CleanScrollView();
            PopulateScrollView();
            CleanDetailPane();
        }
    }

    void CleanScrollView()
    {
        foreach(Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void CleanDetailPane()
    {
        jobNameText.text = string.Empty;
        customerNameText.text = string.Empty;
        cargoText.text = string.Empty;
        rewardText.text = string.Empty;
    }

    void PopulateScrollView()
    {
        foreach(Mission mission in MissionsManager.Instance.missionContainer.missions)
        {
            if(mission.moneyNeededToUnlock <= PlayerManager.Instance.playerData.playerTotalMoneyAcquired
                && !mission.hasBeenAccepted)
            {
                GameObject scrollItem = Instantiate(scrollItemPrefab, scrollViewContent.transform);
                NoticeBoardItem noticeBoardItem = scrollItem.GetComponent<NoticeBoardItem>();
                noticeBoardItem.Init(mission, SelectItem);
            }
        }
    }

    public void SelectItem(Mission missionToSelect)
    {
        selectedMission = missionToSelect;
        jobNameText.text = selectedMission.missionName;
        customerNameText.text = selectedMission.customer;
        cargoText.text = selectedMission.cargo;
        rewardText.text = $"${selectedMission.reward}";
    }
}
