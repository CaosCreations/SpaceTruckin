using UnityEngine;
using UnityEngine.UI;

public class NoticeBoard : UICanvasBase
{
    [Header("Set in prefab")]
    // Left Panel
    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private GameObject scrollItemPrefab;

    // Right Panel
    [SerializeField] private Text jobNameText;
    [SerializeField] private Text customerNameText;
    [SerializeField] private Text cargoText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text rewardText;
    [SerializeField] private Text statsText;
    [SerializeField] private Button acceptJobButton;

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
            selectedMission.AcceptMission();
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
        statsText.SetText(string.Empty);
    }

    private void PopulateScrollView()
    {
        foreach (Mission mission in MissionsManager.Instance.Missions)
        {
            if (!mission.HasBeenAccepted && mission.HasBeenUnlocked)
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
        rewardText.SetText(MissionDetailsUI.BuildRewardString(selectedMission));

        // Set stats text based on pilot traits 
        if (selectedMission.PilotTraitEffects != null && selectedMission.PilotTraitEffects.Effects != null)
        {
            statsText.SetText(PreMissionStatsUI.BuildTraitEffectsStatsText(selectedMission.PilotTraitEffects.Effects));
        }

        acceptJobButton.interactable = true;
    }
}
