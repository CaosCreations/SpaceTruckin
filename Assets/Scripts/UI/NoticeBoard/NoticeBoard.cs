using UnityEngine;
using UnityEngine.UI;

public class NoticeBoard : UICanvasBase
{
    [Header("Set in prefab")]
    // Left Panel
    public GameObject scrollViewContent;
    public GameObject scrollItemPrefab;

    // Right Panel
    public Text jobNameText;
    public Text customerNameText;
    public Text cargoText;
    public Text descriptionText;
    public Text rewardText;
    public Button acceptJobButton;

    [Header("Set in game")]
    public Mission selectedMission;

    private void Start()
    {
        acceptJobButton.onClick.RemoveAllListeners();
        acceptJobButton.onClick.AddListener(AcceptMission);
    }

    private void OnEnable()
    {
        selectedMission = null;
        CleanDetailPanel();
        CleanScrollView();
        PopulateScrollView();
        acceptJobButton.interactable = false;
    }

    private void AcceptMission()
    {
        if (selectedMission != null)
        {
            selectedMission.HasBeenAccepted = true;
            acceptJobButton.interactable = false;
            CleanScrollView();
            PopulateScrollView();
            CleanDetailPanel();
        }
    }

    private void CleanScrollView()
    {
        scrollViewContent.transform.DestroyDirectChildren();
    }

    private void CleanDetailPanel()
    {
        jobNameText.SetText(string.Empty);
        customerNameText.SetText(string.Empty);
        cargoText.SetText(string.Empty);
        descriptionText.SetText(string.Empty);
        rewardText.SetText(string.Empty);
    }

    private void PopulateScrollView()
    {
        foreach(Mission mission in MissionsManager.Instance.Missions)
        {
            if(mission.MoneyNeededToUnlock <= PlayerManager.Instance.TotalMoneyAcquired
                && !mission.HasBeenAccepted)
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
        jobNameText.SetText(selectedMission.Name);
        customerNameText.SetText(selectedMission.Customer);
        cargoText.SetText(selectedMission.Cargo);
        descriptionText.SetText(selectedMission.Description);
        rewardText.SetText(MissionDetailsUI.BuildRewardString(missionToSelect));
        acceptJobButton.interactable = true;
    }
}
